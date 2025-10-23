const allExpensesBtn = document.getElementById("all-expenses");
const allIncomesBtn = document.getElementById("all-incomes");

const addExpenseBtn = document.getElementById("add-expense-btn");
const addIncomeBtn = document.getElementById("add-income-btn");

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
const addTypeOfIncomesBtn = document.getElementById("add-type-of-incomes");

const addNewTypeCintainer = document.getElementById("add-new-type-container");
const addNewTypeInput = document.getElementById("input-name-type");
const listAllTypes = document.getElementById("list-all-types");
const newTypeSubmitButton = document.getElementById("new-type-submit-btn");

const getAllResultList = document.getElementById("get-all-result");

let listArray = [];
let expenseOrIncome = {};
let idOfChangingElement;
let isIncomes = true;
let expensesOrIncomesString = "";

const changeOrDeleteOperations = {
    edit: "edit",
    create: "create"
};
let typeOfOperation = {};

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

const fetchAllExpenses = async () => {
    try {
        const response = await fetch("http://localhost:5119/Expenses/AllExpenses");
        const data = await response.json();
        listArray = data;
    } catch (err) {
        console.log("Error catched", err);
    }
};

const fetchAllIncomes = async () => {
    try {
        const response = await fetch("http://localhost:5119/Incomes/AllIncomes");
        const data = await response.json();
        listArray = data;
    } catch (err) {
        console.log("Error catched", err);
    }
};

const showAllTypeOfExpensesOrIncomes = async () => {

    listArray.forEach((type) => {
        listAllTypes.innerHTML += `
        <div id="${type.id}-${expensesOrIncomesString}-type-div-element"><li class="type-of-expenses-or-incomes">${type.name}</li><button class="delete-button-for-types" id="${type.id}-${expensesOrIncomesString}-type-delete-button">delete</button><button class="edit-button-for-types" id="${type.id}-${expensesOrIncomesString}-type-edit-button">edit</button></div>
        `;
    });
    addNewTypeCintainer.classList.remove("hide");
    addNewTypeCintainer.classList.add("opened");
    document.querySelectorAll('.delete-button-for-types').forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();

            const regex = isIncomes ? /-incomes-type-delete-button/g : /-expenses-type-delete-button/g;
            const idOfElement = e.target.id.replace(regex, "");

            const isError = isIncomes ? await fetchDeleteTypeOfIncomes(Number(idOfElement)) : await fetchDeleteTypeOfExpenses(Number(idOfElement));

            if (isError) {
                resultMessage.classList.remove("hidden");
                resultMessage.classList.add("opened");
                resultMessage.textContent = `Error: ${isError}`;
            }
            else {
                const divForDelete = document.getElementById(`${idOfElement}-${expensesOrIncomesString}-type-div-element`);
                divForDelete.remove();
                clearWorkSpace();

                isIncomes ? await fetchAllTypeOfIncomes() : await fetchAllTypeOfExpenses();
                await showAllTypeOfExpensesOrIncomes();
                resultMessage.classList.remove("hidden");
                resultMessage.classList.add("opened");
                resultMessage.textContent = `Type of ${expensesOrIncomesString} delete`;
            }

        });
    });
    document.querySelectorAll('.edit-button-for-types').forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();


            const regex = isIncomes ? /-incomes-type-edit-button/g : /-expenses-type-edit-button/g;
            const idOfElement = e.target.id.replace(regex, "");
            const divForEdit = document.getElementById(`${idOfElement}-${expensesOrIncomesString}-type-div-element`);
            if (!document.getElementById(`${idOfElement}-${expensesOrIncomesString}-type-div-for-edit`)) {
                const editBlock = document.createElement("div");
                editBlock.id = `${idOfElement}-${expensesOrIncomesString}-type-div-for-edit`;
                editBlock.innerHTML = `
                <label id="${idOfElement}-${expensesOrIncomesString}-type-label-for-edit" for="${idOfElement}-${expensesOrIncomesString}-type-input-for-edit">Enter a new name: </label>
                <input placeholder="Type here..." id="${idOfElement}-${expensesOrIncomesString}-type-input-for-edit">
                <button id="${idOfElement}-${expensesOrIncomesString}-type-button-for-edit">confirm</button>
                `;

                divForEdit.appendChild(editBlock);

                const confirmBtn = document.getElementById(`${idOfElement}-${expensesOrIncomesString}-type-button-for-edit`);
                const editInput = document.getElementById(`${idOfElement}-${expensesOrIncomesString}-type-input-for-edit`);
                confirmBtn.addEventListener("click", async (e) => {
                    e.preventDefault();

                    const currentTypeName = listArray.find((type) => type.id === Number(idOfElement)).name;

                    console.log(currentTypeName);
                    if (editInput.value.trim().toLowerCase() !== currentTypeName.toLowerCase()) {
                        const newType = {
                            nameOfType: editInput.value.trim()
                        };
                        const isError = isIncomes ? await fetchUpdateTypeOfIncomes(newType, Number(idOfElement)) : await fetchUpdateTypeOfExpenses(newType, Number(idOfElement));
                        if (isError) {
                            resultMessage.textContent = `Error: ${isError}`;
                        }
                        else {
                            divForEdit.remove();
                            clearWorkSpace();
                            isIncomes ? await fetchAllTypeOfIncomes() : await fetchAllTypeOfExpenses();
                            await showAllTypeOfExpensesOrIncomes();
                            resultMessage.classList.remove("hidden");
                            resultMessage.classList.add("opened");
                            resultMessage.textContent = `Name changed`;
                        }
                    }
                    else {
                        alert(`${editInput.value.trim()} name is exist, try another name`);
                    }
                })
            }

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

const fetchUpdateTypeOfIncomes = async (newType, id) => {
    try {
        const response = await fetch(`http://localhost:5119/IncomesType/${id}`,
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
        if (!response.ok) {
            console.log('Error:', response.status);
            return response.statusText;
        }
    } catch (err) {
        console.log("Error catched", err);
    }
}

const fetchAllTypeOfIncomes = async () => {
    try {
        const response = await fetch("http://localhost:5119/IncomesTypes/TypesOfIncomes");
        const data = await response.json();
        listArray = data;
        if (!response.ok) {
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

const postIncome = async (income) => {
    try {
        const response = await fetch("http://localhost:5119/Incomes",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(income)
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
        const response = await fetch("http://localhost:5119/IncomesTypes",
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

const fetchDeleteIncome = async (id) => {
    try {
        const response = await fetch(`http://localhost:5119/Incomes/${id}`,
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

const fetchPutExpense = async (newExpense, id) => {
    try {
        const response = await fetch(`http://localhost:5119/Expenses/${id}`,
            {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(newExpense)
            }
        );
        if (!response) {
            console.log(`Error:`, response.status);
            return response.statusText;
        }
    } catch (err) {
        console.log(`Error:`, err);
    }
}

const fetchPutIncome = async (newIncome, id) => {
    try {
        const response = await fetch(`http://localhost:5119/Incomes/${id}`,
            {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(newIncome)
            }
        );
        if (!response) {
            console.log(`Error:`, response.status);
            return response.statusText;
        }
    } catch (err) {
        console.log(`Error:`, err);
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
        console.log(id);
        const response = await fetch(`http://localhost:5119/IncomesTypes/${id}`,
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

submitBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    const amount = amountInput.value;
    const type = selectType.value;
    const comment = commentInput.value;

    if (!isIncomes && amount && type && typeOfOperation === changeOrDeleteOperations.create) {
        expenseOrIncome = {
            amount: amount,
            type: type,
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
            type: type,
            comment: comment
        }
        const isError = await fetchPutExpense(expenseOrIncome, Number(idOfChangingElement));
        if (isError) {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = `Error: ${isError}`;
        }
        else {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = "The expense was edited!";
        }
    }
    else if (isIncomes && amount && type && typeOfOperation === changeOrDeleteOperations.create) {
        expenseOrIncome = {
            amount: amount,
            type: type,
            comment: comment
        }
        const isError = await postIncome(expenseOrIncome);
        console.log(isError);
        if (isError) {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = `Error: ${isError}`;
        }
        else {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = "The income was edded!";
        }
    }
    else if (isIncomes && amount && type && typeOfOperation === changeOrDeleteOperations.edit) {
        expenseOrIncome =
        {
            amount: amount,
            type: type,
            comment: comment
        }
        const isError = await fetchPutIncome(expenseOrIncome, Number(idOfChangingElement));
        if (isError) {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = `Error: ${isError}`;
        }
        else {
            resultMessage.classList.remove("hidden");
            resultMessage.classList.add("opened");
            resultMessage.textContent = "The income was edited!";
        }
    }
    idOfChangingElement = "";
});

const showAddExpenseOrIncomeMenu = async () => {
    clearWorkSpace();
    formContainer.classList.remove("hidden");
    if (!isIncomes) {
        await fetchAllTypeOfExpenses();
    }
    else {
        await fetchAllTypeOfIncomes();
    }

    listArray.forEach((type) => {
        selectType.innerHTML += `
        <option value="${type.name}">${type.name}</option>
        `;
    });
}

addExpenseBtn.addEventListener("click", async () => {
    isIncomes = false;
    typeOfOperation = changeOrDeleteOperations.create;
    await showAddExpenseOrIncomeMenu();
})

addIncomeBtn.addEventListener("click", async () => {
    isIncomes = true;
    typeOfOperation = changeOrDeleteOperations.create;
    await showAddExpenseOrIncomeMenu();
})

expensesMenuBtn.addEventListener("click", () => {
    clearWorkSpace();
    incomesMenu.classList.add("hidden");
    expensesMenu.classList.toggle("hidden");
})

incomesMenuBtn.addEventListener("click", () => {
    clearWorkSpace();
    expensesMenu.classList.add("hidden");
    incomesMenu.classList.toggle("hidden");
})

const showAllExpensesOrIncomes = async () => {
    isIncomes ? await fetchAllIncomes() : await fetchAllExpenses();
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
    <div id="${el.id}-${expensesOrIncomesString}-div-element">
        <li class="position-of-expense-or-income" id="${el.id}">${timePast ? timePast : date} Amount: $${el.amount} Type: ${el.type} ${el.comment ? "Comment:" + el.comment : ""}</li>
        <button class="delete-button-for-expense-or-income" id="${el.id}-expense-or-income-delete-button">delete</button>
        <button class="edit-button-for-expense-or-income" id="${el.id}-edit-button-for-expense-or-income">edit</button>
    </div>
    `;
    })
    document.querySelectorAll(".delete-button-for-expense-or-income").forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();
            const regex = /-expense-or-income-delete-button/g;
            const idOfElement = e.target.id.replace(regex, "");
            const result = isIncomes ? await fetchDeleteIncome(idOfElement) : await fetchDeleteExpense(idOfElement);
            if (result) {
                alert('Error:', result);
            }
            else {
                clearWorkSpace();
                await showAllExpensesOrIncomes();
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
            idOfChangingElement = idOfElement;
        })
    })
}

allExpensesBtn.addEventListener("click", async () => {
    clearWorkSpace();
    isIncomes = false;
    expensesOrIncomesString = "expense";
    showAllExpensesOrIncomes();
});

allIncomesBtn.addEventListener("click", async () => {
    clearWorkSpace();
    isIncomes = true;
    expensesOrIncomesString = "income";
    showAllExpensesOrIncomes();
})

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
        await showAllTypeOfExpensesOrIncomes();
        resultMessage.classList.remove("hidden");
        resultMessage.classList.add("opened");
        resultMessage.textContent = "The expenses type was edded!!!";
    }
    else if (isOrigin && isIncomes) {
        const type = { nameOfType: addNewTypeInput.value.trim() };
        await postTypeOfIncome(type);
        clearWorkSpace();
        await fetchAllTypeOfIncomes();
        await showAllTypeOfExpensesOrIncomes();
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
    const result = await fetchAllTypeOfExpenses();
    if (!result) {
        expensesOrIncomesString = "expenses";
        isIncomes = false;
        await showAllTypeOfExpensesOrIncomes();
    }
});

addTypeOfIncomesBtn.addEventListener("click", async () => {
    clearWorkSpace();
    const result = await fetchAllTypeOfIncomes();
    if (!result) {
        expensesOrIncomesString = "incomes";
        isIncomes = true;
        await showAllTypeOfExpensesOrIncomes();
    }
});






