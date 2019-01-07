let foodI = 0;

let placeI = 0;

const parentDiv = document.querySelector(".experience-container");

parentDiv.addEventListener("click", (e) => {

    if (e.target.id === "add-food-btn" || e.target.id === "add-place-btn") {
        e.preventDefault();

        let index = 0;

        let type = e.target.id.split("-")[1];
        let Type = type.charAt(0).toUpperCase() + type.slice(1);

        let container = document.querySelector(`#new-${type}-container`);

        if (type === "food") {
            index = foodI
        } else {
            index = placeI
        }

        let newCard = document.createElement("div");
        if (type === "food") {
            newCard.setAttribute("class", "card p-2 bg-light mt-2 mb-2 inset-shadow-orange");
        } else {
            newCard.setAttribute("class", "card p-2 bg-light mt-2 mb-2 inset-shadow-green");
        }

        let innerDiv = document.createElement("div");
        innerDiv.setAttribute("class", "d-flex justify-content-start");

        let leftDiv = document.createElement("div");
        leftDiv.setAttribute("class", "width-10 mr-2 d-flex flex-column justify-content-center");

        let rightDiv = document.createElement("div");
        rightDiv.setAttribute("class", "width-85");

        let inputCheckbox = document.createElement("input");
        inputCheckbox.type = "checkbox";
        inputCheckbox.setAttribute("checked", "checked")
        inputCheckbox.setAttribute("value", true);
        inputCheckbox.setAttribute("data-val", true)
        inputCheckbox.setAttribute("name", `New${Type}s[${index}].IsCompleted`)

        leftDiv.appendChild(inputCheckbox);


        let nameInput = document.createElement("input")
        nameInput.setAttribute("Placeholder", "Name")
        nameInput.setAttribute("id", `${Type}-name-${index + 1}`)
        nameInput.setAttribute("class", "form-control mb-2")
        nameInput.setAttribute("data-val", true)
        nameInput.setAttribute("data-val-required", "A Name is required")
        nameInput.setAttribute("name", `New${Type}s[${index}].Name`)

        let nameValidator = document.createElement("span");
        nameValidator.setAttribute("class", "text-danger field-validation-valid");
        nameValidator.setAttribute("data-valmsg-for", `${Type}-name-${index + 1}`);
        nameValidator.setAttribute("data-valmsg-replace", true);

        let descTextarea = document.createElement("textarea")
        descTextarea.setAttribute("Placeholder", "Description")
        descTextarea.setAttribute("class", "form-control")
        descTextarea.setAttribute("id", `${Type}-desc-${index + 1}`)
        descTextarea.setAttribute("data-val", true)
        descTextarea.setAttribute("name", `New${Type}s[${index}].Description`)

        rightDiv.appendChild(nameInput);
        rightDiv.appendChild(nameValidator);
        rightDiv.appendChild(descTextarea);

        innerDiv.appendChild(leftDiv);
        innerDiv.appendChild(rightDiv);

        newCard.appendChild(innerDiv);

        container.appendChild(newCard);

        if (type === "food") {
            return foodI++
        } else {
            return placeI++
        }

    }

})