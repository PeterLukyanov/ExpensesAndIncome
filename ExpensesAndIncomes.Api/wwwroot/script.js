const operationsMenu = document.getElementById("operations-menu-buttons-container");

const expensesMenuBtn = document.getElementById("expenses-menu-btn");
const incomesMenuBtn = document.getElementById("incomes-menu-btn");

const allOperationsBtn = document.getElementById("all-operations");
const addOperationBtn = document.getElementById("add-operation-btn");
const addTypeOfOperationsBtn = document.getElementById("types-menu");

const formContainer = document.getElementById("form-for-add-operation");
const amountInput = document.getElementById("amount-input");
const commentInput = document.getElementById("comment-input");
const selectType = document.getElementById("select-type");
const submitBtn = document.getElementById("submit-btn");
const outputAndInputZone = document.getElementById("output-and-input-zone");

const resultMessage = document.getElementById("result-message");

const addNewTypeContainer = document.getElementById("add-new-type-container");
const addNewTypeInput = document.getElementById("input-name-type");
const listAllTypes = document.getElementById("list-all-types");
const newTypeSubmitButton = document.getElementById("new-type-submit-btn");

const getAllResultList = document.getElementById("get-all-result");

let listArray = [];
let idOfChangingElement;
let isIncomes = false;

const changeOrDeleteOperations = {
    edit: "edit",
    create: "create"
};
let typeOfOperation = {};

const clearWorkSpace = () => {
    formContainer.classList.add("hidden");
    listArray = [];
    amountInput.value = 0;
    selectType.value = "";
    selectType.innerHTML = `<option value="" selected disabled>(Select one)</option>`;
    commentInput.value = "";
    resultMessage.classList.remove("opened");
    resultMessage.classList.add("hidden");
    resultMessage.textContent = "";
    addNewTypeContainer.classList.remove("opened");
    addNewTypeContainer.classList.add("hidden");
    listAllTypes.innerHTML = "";
    getAllResultList.classList.remove("opened");
    getAllResultList.classList.add("hidden");
    getAllResultList.innerHTML = "";
    addNewTypeInput.value = "";
}

const changeBtns = () => {
    allOperationsBtn.textContent = isIncomes ? "All Incomes" : "All Expenses";
    addOperationBtn.textContent = isIncomes ? "Add Income" : "Add Expense";
}

const fetchAllOperations = async () => {
    try {
        const response = isIncomes ? await fetch("http://localhost:5119/Incomes/AllIncomes") : await fetch("http://localhost:5119/Expenses/AllExpenses");
        const data = await response.json();
        listArray = data;
    } catch (err) {
        console.log("Error catched", err);
    }
};

const showAllTypeOfExpensesOrIncomes = async () => {

    listArray.forEach((type) => {
        listAllTypes.innerHTML += `
        <div id="${type.id}-operations-type-div-element">
            <li class="type-of-operations">${type.name}</li>
            <button class="delete-button-for-types" id="${type.id}-operations-type-delete-button">delete</button>
            <button class="edit-button-for-types" id="${type.id}-operations-type-edit-button">edit</button>
        </div>
        `;
    });
    addNewTypeContainer.classList.remove("hide");
    addNewTypeContainer.classList.add("opened");
    document.querySelectorAll('.delete-button-for-types').forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();

            const regex = /-operations-type-delete-button/g;
            const idOfElement = e.target.id.replace(regex, "");

            const isError = await fetchDeleteTypeOfOperations(Number(idOfElement));

            if (isError) {
                resultMessage.classList.remove("hidden");
                resultMessage.classList.add("opened");
                resultMessage.textContent = `Error: ${isError}`;
            }
            else {
                const divForDelete = document.getElementById(`${idOfElement}-operations-type-div-element`);
                divForDelete.remove();
                clearWorkSpace();

                await fetchAllTypeOfOperations();
                await showAllTypeOfExpensesOrIncomes();
                resultMessage.classList.remove("hidden");
                resultMessage.classList.add("opened");
                resultMessage.textContent = `Type of ${isIncomes ? 'income' : 'expense'} delete`;
            }

        });
    });
    document.querySelectorAll('.edit-button-for-types').forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();


            const regex =/-operations-type-edit-button/g;
            const idOfElement = e.target.id.replace(regex, "");
            const divForEdit = document.getElementById(`${idOfElement}-operations-type-div-element`);
            if (!document.getElementById(`${idOfElement}-operations-type-div-for-edit`)) {
                const editBlock = document.createElement("div");
                editBlock.id = `${idOfElement}-operations-type-div-for-edit`;
                editBlock.innerHTML = `
                <label id="${idOfElement}-operations-type-label-for-edit" for="${idOfElement}-operations-type-input-for-edit">Enter a new name: </label>
                <input placeholder="Type here..." id="${idOfElement}-operations-type-input-for-edit">
                <button id="${idOfElement}-operations-type-button-for-edit">confirm</button>
                `;

                divForEdit.appendChild(editBlock);

                const confirmBtn = document.getElementById(`${idOfElement}-operations-type-button-for-edit`);
                const editInput = document.getElementById(`${idOfElement}-operations-type-input-for-edit`);
                confirmBtn.addEventListener("click", async (e) => {
                    e.preventDefault();

                    const currentTypeName = listArray.find((type) => type.id === Number(idOfElement)).name;

                    console.log(currentTypeName);
                    if (editInput.value.trim().toLowerCase() !== currentTypeName.toLowerCase()) {
                        const newType = {
                            nameOfType: editInput.value.trim()
                        };
                        const isError = await fetchUpdateTypeOfOperations(newType, Number(idOfElement));
                        if (isError) {
                            resultMessage.textContent = `Error: ${isError}`;
                        }
                        else {
                            divForEdit.remove();
                            clearWorkSpace();
                            await fetchAllTypeOfOperations();
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

const fetchUpdateTypeOfOperations = async (newType, id) => {
    try {
        const response = await fetch(isIncomes ? `http://localhost:5119/IncomesTypes/${id}` : `http://localhost:5119/ExpensesTypes/${id}`,
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

const fetchAllTypeOfOperations = async () => {
    try {
        const response = await fetch(isIncomes ? "http://localhost:5119/IncomesTypes/TypesOfIncomes" : "http://localhost:5119/ExpensesTypes/TypesOfExpenses");
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

const postOperation = async (operation) => {
    try {
        const response = await fetch(isIncomes ? "http://localhost:5119/Incomes" : "http://localhost:5119/Expenses",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(operation)
            });
        if (!response.ok) {
            console.log(`Error code: ${response.status}`);
            return response.statusText;
        }
    } catch (err) {
        console.log("Error", err);
    }

}

const postTypeOfOperations = async (type) => {
    try {
        const response = await fetch(isIncomes ? "http://localhost:5119/IncomesTypes" : "http://localhost:5119/ExpensesTypes",
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

const fetchDeleteOperation = async (id) => {
    try {
        const response = await fetch(isIncomes ? `http://localhost:5119/Incomes/${id}` : `http://localhost:5119/Expenses/${id}`,
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

const fetchPutOperation = async (newIncome, id) => {
    try {
        const response = await fetch((isIncomes ? `http://localhost:5119/Incomes/${id}` : `http://localhost:5119/Expenses/${id}`),
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

const fetchDeleteTypeOfOperations = async (id) => {
    try {
        const response = await fetch(isIncomes ? `http://localhost:5119/IncomesTypes/${id}` : `http://localhost:5119/ExpensesTypes/${id}`,
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

    if (!(amount && type)) {
        return;
    }

    var operation = {
        amount: amount,
        type: type,
        comment: comment
    }

    resultMessage.classList.remove("hidden");
    resultMessage.classList.add("opened");

    if (typeOfOperation === changeOrDeleteOperations.create) {

        const isError = await postOperation(operation);
        if (isError) {
            resultMessage.textContent = `Error: ${isError}`;
        }
        else {
            resultMessage.textContent = `The ${isIncomes ? 'income' : 'expense'} was edded!`;
        }
    }
    else if (typeOfOperation === changeOrDeleteOperations.edit) {

        const isError = await fetchPutOperation(operation, Number(idOfChangingElement));
        if (isError) {
            resultMessage.textContent = `Error: ${isError}`;
        }
        else {
            resultMessage.textContent = `The ${isIncomes ? 'income' : 'expense'} was edited!`;
        }
    }
    idOfChangingElement = "";
});

const showAddExpenseOrIncomeMenu = async () => {
    clearWorkSpace();
    formContainer.classList.remove("hidden");

    await fetchAllTypeOfOperations();

    listArray.forEach((type) => {
        selectType.innerHTML += `
        <option value="${type.name}">${type.name}</option>
        `;
    });
}

addOperationBtn.addEventListener("click", async () => {
    typeOfOperation = changeOrDeleteOperations.create;
    await showAddExpenseOrIncomeMenu();
})

expensesMenuBtn.addEventListener("click", async() => {
    clearWorkSpace();
    isIncomes = false;
    await showAllExpensesOrIncomes();
    changeBtns();
})

incomesMenuBtn.addEventListener("click", async() => {
    clearWorkSpace();
    isIncomes = true;
    await showAllExpensesOrIncomes();
    changeBtns();
})

const showAllExpensesOrIncomes = async () => {
    await fetchAllOperations();
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
    <div id="${el.id}-${isIncomes ? 'income' : 'expense'}-div-element">
        <li class="position-of-operation" id="${el.id}">${timePast ? timePast : date} Amount: $${el.amount} Type: ${el.type} ${el.comment ? "Comment:" + el.comment : ""}</li>
        <button class="delete-button-for-operation" id="${el.id}-operation-delete-button">delete</button>
        <button class="edit-button-for-operation" id="${el.id}-edit-button-for-operation">edit</button>
    </div>
    `;
    })
    document.querySelectorAll(".delete-button-for-operation").forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();
            const regex = /-operation-delete-button/g;
            const idOfElement = e.target.id.replace(regex, "");
            const result = await fetchDeleteOperation(idOfElement);
            if (result) {
                alert('Error:', result);
            }
            else {
                clearWorkSpace();
                await showAllExpensesOrIncomes();
            }
        })
    })
    document.querySelectorAll(".edit-button-for-operation").forEach((button) => {
        button.addEventListener("click", async (e) => {
            e.preventDefault();
            typeOfOperation = changeOrDeleteOperations.edit;
            const regex = /-edit-button-for-operation/g;
            const idOfElement = e.target.id.replace(regex, "");
            idOfChangingElement = idOfElement;
            await showAddExpenseOrIncomeMenu();
        })
    })
}

allOperationsBtn.addEventListener("click", async () => {
    clearWorkSpace();
    showAllExpensesOrIncomes();
})

newTypeSubmitButton.addEventListener("click", async (e) => {
    e.preventDefault();
    let isOrigin = true;
    listArray.forEach((type) => {
        if (addNewTypeInput.value.trim().toLowerCase() !== type.name.trim().toLowerCase()) {
            isOrigin = true;
        }
        else {
            isOrigin = false;
        }
    });

    if (isOrigin) {
        const type = { nameOfType: addNewTypeInput.value.trim() };
        await postTypeOfOperations(type);
        clearWorkSpace();
        await fetchAllTypeOfOperations();
        await showAllTypeOfExpensesOrIncomes();
        resultMessage.classList.remove("hidden");
        resultMessage.classList.add("opened");
        resultMessage.textContent = `The ${isIncomes ? 'incomes' : 'expenses'} type was edded!!!`;
    }
    else {
        resultMessage.classList.remove("hidden");
        resultMessage.classList.add("opened");
        resultMessage.textContent = "This type exists!";
    }
})

addTypeOfOperationsBtn.addEventListener("click", async () => {
    clearWorkSpace();
    const result = await fetchAllTypeOfOperations();
    if (!result) {
        await showAllTypeOfExpensesOrIncomes();
    }
});

(async()=>await showAllExpensesOrIncomes())();






