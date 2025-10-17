const allExpensesBtn = document.getElementById("all-expenses");
const allIncomesBtn = document.getElementById("all-incomes");
const addExpenseBtn = document.getElementById("add-expense-btn")
const formContainer = document.getElementById("form-for-expenses-and-incomes");
const amountInput = document.getElementById("amount-input");
const commentInput = document.getElementById("comment-input");
const selectType = document.getElementById("select-type");
const submitBtn = document.getElementById("submit-btn");
const outputAndInputZone = document.getElementById("output-and-input-zone");
const expensesMenuBtn = document.getElementById("expenses-menu-btn");
const incomesMenuBtn = document.getElementById("incomes-menu-btn");
const expensesMenu = document.getElementById("expenses-menu-buttons-container");
const incomesMenu = document.getElementById("incomes-menu-buttons-container");
const resultMessage = document.getElementById("result-message");
const addTypeOfExpensesBtn = document.getElementById("add-type-of-expenses");
const addNewTypeCintainer = document.getElementById("add-new-type-container");
const addNewTypeInput = document.getElementById("input-name-type");
const listAllTypes = document.getElementById("list-all-types");
const newTypeSubmitButton = document.getElementById("new-type-submit-btn");

const getAllResultList = document.getElementById("get-all-result");

let listArray = [];
let expenseOrIncome = {};
let idOfChangingElement;
let isIncomes = true;

const changeOrDeleteOperations = {
    edit: "edit",
    create: "create"
};
let typeOfOperation = {};

const fetchAllExpenses = async () => {
    try {
        const response = await fetch("http://localhost:5119/Expenses/AllExpenses");
        const data = await response.json();
        listArray = data;
    } catch (err) {
        console.log("Error catched", err);
    }
};

const showAllTypeOfExpenses = async () => {
    isIncomes = false;
    listArray.forEach((type) => {
        listAllTypes.innerHTML += `
        <div id="${type.id}-expenses-type-div-element"><li class="type-of-expenses-or-incomes">${type.name}</li><button class="delete-button-for-types" id="${type.id}-expenses-type-delete-button">delete</button><button class="edit-button-for-types" id="${type.id}-expenses-type-edit-button">edit</button></div>
        `;
    });
    addNewTypeCintainer.classList.remove("hide");
    addNewTypeCintainer.classList.add("opened");
    document.querySelectorAll('.delete-button-for-types').forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();
            const regex = /-expenses-type-delete-button/g;
            const idOfElement = e.target.id.replace(regex, "");

            const isError = await fetchDeleteTypeOfExpenses(Number(idOfElement));
            if (isError) {
                resultMessage.classList.remove("hidden");
                resultMessage.classList.add("opened");
                resultMessage.textContent = `Error: ${isError}`;
            }
            else {
                const divForDelete = document.getElementById(`${idOfElement}-expenses-type-div-element`);
                divForDelete.remove();
                clearWorkSpace();
                await fetchAllTypeOfExpenses();
                await showAllTypeOfExpenses();
                resultMessage.classList.remove("hidden");
                resultMessage.classList.add("opened");
                resultMessage.textContent = `Expenses type delete`;
            }

        });
    });
    document.querySelectorAll('.edit-button-for-types').forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();

            const regex = /-expenses-type-edit-button/g;
            const idOfElement = e.target.id.replace(regex, "");
            const divForEdit = document.getElementById(`${idOfElement}-expenses-type-div-element`);
            divForEdit.innerHTML += `
            <div id="${idOfElement}-expenses-type-div-for-edit">
                <label id="${idOfElement}-expenses-type-label-for-edit" for="${idOfElement}-expenses-type-input-for-edit">Enter a new name: </label>
                <input placeholder="Type here..." id="${idOfElement}-expenses-type-input-for-edit">
                <button id="${idOfElement}-expenses-type-button-for-edit">confirm</button>
            </div>
            `;
            const confirmBtn = document.getElementById(`${idOfElement}-expenses-type-button-for-edit`);
            const editInput = document.getElementById(`${idOfElement}-expenses-type-input-for-edit`);
            confirmBtn.addEventListener("click", async (e) => {
                e.preventDefault();

                const currentTypeName = listArray.find((type) => type.id === Number(idOfElement)).name;

                console.log(currentTypeName);
                if (editInput.value.trim().toLowerCase() !== currentTypeName.toLowerCase()) {
                    const newType = {
                        nameOfType: editInput.value.trim()
                    };
                    const isError = await fetchUpdateTypeOfExpenses(newType, Number(idOfElement));
                    if (isError) {
                        resultMessage.textContent = `Error: ${isError}`;
                    }
                    else {
                        divForEdit.remove();
                        clearWorkSpace();
                        await fetchAllTypeOfExpenses();
                        await showAllTypeOfExpenses();
                        resultMessage.classList.remove("hidden");
                        resultMessage.classList.add("opened");
                        resultMessage.textContent = `Name changed`;
                    }
                }
                else {
                    alert(`${editInput.value.trim()} name is exist, try another name`);
                }
            })

        })
    });


}

const fetchUpdateTypeOfExpenses = async (newType, id) => {
    try {
        const response = await fetch(`http://localhost:5119/ExpensesTypes/${id}`,
            {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(newType)
            }
        );
        if (!response.ok) {
            console.log(response.status);
            return response.statusText;
        }
    } catch (err) {
        console.log("Error:", err);
    }
}

const fetchAllTypeOfExpenses = async () => {
    try {
        const response = await fetch("http://localhost:5119/ExpensesTypes/TypesOfExpenses");
        const data = await response.json();
        listArray = data;
        if(!response.ok)
        {
            console.log('Error:', response.status);
            return response.statusText;
        }
    } catch (err) {
        console.log("Error catched", err);
    }
}

const postExpense = async (expense) => {
    try {
        const response = await fetch("http://localhost:5119/Expenses",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(expense)
            });
        if (!response.ok) {
            console.log(`Error code: ${response.status}`);
            return response.statusText;
        }
    } catch (err) {
        console.log("Error", err);
    }

}

const postTypeOfExpense = async (type) => {
    try {
        const response = await fetch("http://localhost:5119/ExpensesTypes",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(type)
            });
    } catch (err) {
        console.log("Error", err);
    }
}

const postTypeOfIncome = async (type) => {
    try {
        const response = await fetch("http://localhost:5119/IncomesType",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(type)
            }
        );
    } catch (err) {
        console.log("Error", err);
    }
}

const fetchDeleteExpense = async (id) => {
    try {
        const response = await fetch(`http://localhost:5119/Expenses/${id}`,
            {
                method: "DELETE"
            }
        );
        if (!response.ok) {
            console.log("Error:", response.status);
            return response.statusText;
        }
    } catch (err) {
        console.log(`Error:`, err);
    }
};

const fetchPutExpense = async(newExpense, id)=>{
    try{
        const response = await fetch(`http://localhost:5119/Expenses/${id}`,
            {
                method: "PUT",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(newExpense)
            }
        );
        if(!response)
        {
            console.log(`Error:`, response.status);
            return response.statusText;
        }
    }catch(err){
        console.log(`Error:`,err);
    }
}

const fetchDeleteTypeOfExpenses = async (id) => {
    try {
        const response = await fetch(`http://localhost:5119/ExpensesTypes/${id}`,
            {
                method: "DELETE"
            });
        if (!response.ok) {
            console.log("Error:", response.status);
            return response.statusText;
        }
    } catch (err) {
        console.log(`Error:`, err);
    }
};

const fetchDeleteTypeOfIncomes = async (id) => {
    try {
        const response = await fetch(`http://localhost:5119/IncomesType/${id}`,
            {
                method: "DELETE"
            });
        if (!response.ok) {
            console.log("Error:", response.status);
            return response.statusText;
        }
    } catch (err) {
        console.log(`Error:`, err);
    }
};

const clearWorkSpace = () => {
    formContainer.classList.add("hidden");
    listArray = [];
    expenseOrIncome = {};
    amountInput.value = 0;
    selectType.value = "";
    selectType.innerHTML = `<option value="" selected disabled>(Select one)</option>`;
    commentInput.value = "";
    resultMessage.classList.remove("opened");
    resultMessage.classList.add("hidden");
    resultMessage.textContent = "";
    addNewTypeCintainer.classList.remove("opened");
    addNewTypeCintainer.classList.add("hidden");
    listAllTypes.innerHTML = "";
    getAllResultList.classList.remove("opened");
    getAllResultList.classList.add("hidden");
    getAllResultList.innerHTML = "";
    addNewTypeInput.value = "";
}

submitBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    const amount = amountInput.value;
    const type = selectType.value;
    const comment = commentInput.value;

    if (!isIncomes && amount && type && typeOfOperation === changeOrDeleteOperations.create) {
        expenseOrIncome = {
            amount: amount,
            typeOfExpenses: type,
            comment: comment
        }
        const isError = await postExpense(expenseOrIncome);
        console.log(isError);
        if (isError) {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = `Error: ${isError}`;
        }
        else {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = "The expense was edded!";
        }
    }
    else if (!isIncomes && amount && type && typeOfOperation === changeOrDeleteOperations.edit) {
        expenseOrIncome = 
        {
            amount: amount,
            typeOfExpenses: type,
            comment: comment
        }
        const isError = await fetchPutExpense(expenseOrIncome, Number(idOfChangingElement));
        if(isError)
        {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = `Error: ${isError}`;
        }
        else{
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = "The expense was edited!";
        }
    }
    idOfChangingElement="";
});

const showAddExpenseOrIncomeMenu = async () => {
    clearWorkSpace();
    formContainer.classList.remove("hidden");
    if (!isIncomes) {
        await fetchAllTypeOfExpenses();
    }
    else {

    }

    listArray.forEach((type) => {
        selectType.innerHTML += `
        <option value="${type.name}">${type.name}</option>
        `;
    });
}

addExpenseBtn.addEventListener("click", async () => {
    isIncomes = false;
    typeOfOperation=changeOrDeleteOperations.create;
    await showAddExpenseOrIncomeMenu();
})

expensesMenuBtn.addEventListener("click", (e) => {
    //e.preventDefault();
    clearWorkSpace();
    incomesMenu.classList.add("hidden");
    expensesMenu.classList.toggle("hidden");
})

incomesMenuBtn.addEventListener("click", (e) => {
    //e.preventDefault();
    clearWorkSpace();
    expensesMenu.classList.add("hidden");
    incomesMenu.classList.toggle("hidden");
})

const showAllExpenses = async () => {
    isIncomes = false;
    await fetchAllExpenses();
    listArray.sort((a, b) => new Date(b.dataOfAction) - new Date(a.dataOfAction));
    listArray.forEach((el) => {
        let timePastSeconds = (Date.now() / 1000) - (new Date(el.dataOfAction)) / 1000;

        timePastSeconds = Math.floor(timePastSeconds);

        let timePast = "";
        let date = "";
        if (timePastSeconds < 60) {
            timePast = `${timePastSeconds}s ago`;
        }
        else if (timePastSeconds < 60 * 60) {
            timePast = `${Math.floor(timePastSeconds / 60)}m ago`;
        }
        else if (timePastSeconds < 60 * 60 * 24) {
            timePast = `${Math.floor(timePastSeconds / 60 / 60)}h ago`;
        }
        else if (timePastSeconds < 60 * 60 * 24 * 30) {
            timePast = `${Math.floor(timePastSeconds / 60 / 60 / 24)}d ago`;
        }
        else {
            const fullDate = new Date(el.dataOfAction);
            const day = String((fullDate.getDate())).padStart(2, "0");
            const month = String((fullDate.getMonth() + 1)).padStart(2, "0");
            const year = String((fullDate.getFullYear()));
            date = `${day}.${month}.${year}`;
        }
        console.log(el.comment.value);
        getAllResultList.classList.remove("hidden");
        getAllResultList.classList.add("opened");
        getAllResultList.innerHTML += `
    <div id="${el.id}-expense-div-element"><li class="position-of-expenses-or-incomes" id="${el.id}">${timePast ? timePast : date} Amount: $${el.amount} Type: ${el.typeOfExpenses} ${el.comment ? "Comment:" + el.comment : ""}</li>
    <button class="delete-button-for-expense-or-income" id="${el.id}-expense-or-income-delete-button">delete</button><button class="edit-button-for-expense-or-income" id="${el.id}-edit-button-for-expense-or-income">edit</button></div>
    `;
    })
    document.querySelectorAll(".delete-button-for-expense-or-income").forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();
            const regex = /-expense-or-income-delete-button/g;
            const idOfElement = e.target.id.replace(regex, "");
            const result = await fetchDeleteExpense(idOfElement);
            if (result) {
                alert('Error:', result);
            }
            else {
                clearWorkSpace();
                await showAllExpenses();
            }
        })
    })
    document.querySelectorAll(".edit-button-for-expense-or-income").forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();
            typeOfOperation = changeOrDeleteOperations.edit;
            const regex = /-edit-button-for-expense-or-income/g;
            const idOfElement = e.target.id.replace(regex, "");
            await showAddExpenseOrIncomeMenu();
            idOfChangingElement=idOfElement;
        })
    })
}

allExpensesBtn.addEventListener("click", async () => {
    clearWorkSpace();
    showAllExpenses();
});

newTypeSubmitButton.addEventListener("click", async (e) => {
    e.preventDefault();
    let isOrigin = true;
    listArray.forEach((type) => {
        if (addNewTypeInput.value.trim().toLowerCase() !== type.name.trim().toLowerCase()) {
            isOrigin = true;
            console.log("true");
        }
        else {
            isOrigin = false;
            console.log("false");
        }
    });

    if (isOrigin && !isIncomes) {
        const type = { nameOfType: addNewTypeInput.value.trim() };
        await postTypeOfExpense(type);
        clearWorkSpace();
        await fetchAllTypeOfExpenses();
        await showAllTypeOfExpenses();
        resultMessage.classList.remove("hidden");
        resultMessage.classList.add("opened");
        resultMessage.textContent = "The expenses type was edded!!!";
    }
    else if (isOrigin && isIncomes) {
        const type = { nameOfType: addNewTypeInput.value.trim() };
        await postTypeOfIncome(type);
        resultMessage.classList.remove("hidden");
        resultMessage.classList.add("opened");
        resultMessage.textContent = "The incomes type was edded!!!";
    }
    else {
        resultMessage.classList.remove("hidden");
        resultMessage.classList.add("opened");
        resultMessage.textContent = "This type exists!";
    }
})

addTypeOfExpensesBtn.addEventListener("click", async () => {
    clearWorkSpace();
    const result= await fetchAllTypeOfExpenses();
    if(!result)
    {
        await showAllTypeOfExpenses();
    }
});






