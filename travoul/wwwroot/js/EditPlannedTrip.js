const locationContainer = document.querySelector(".plan-create-locations-card");

let foodI = document.querySelector(".food-container").children.length;

let visitI = document.querySelector(".visit-container").children.length;

locationContainer.addEventListener("click", (e) => {
    e.preventDefault();
    if (e.target.id === "add-food-btn" || e.target.id === "add-visit-btn") {
        let i = 0;

        let type = e.target.id.split("-")[1]

        let Type = type.charAt(0).toUpperCase() + type.slice(1);

        if (type === "food") {
            i = foodI
        } else if (type === "visit") {
            i = visitI
        }

        let container = document.querySelector(`.${type}-container`);

        let newParent = document.createElement("div");
        newParent.setAttribute("class", "card p-2 bg-light mb-2");
        newParent.setAttribute("id", `${type}-location-${i + 1}`);

        let innerDiv = document.createElement("div");
        let buttonDiv = document.createElement("div");
        buttonDiv.setAttribute("class", "d-flex justify-content-end");

        let removeBtn = document.createElement("button");
        removeBtn.setAttribute("class", "btn btn-sm btn-danger");
        removeBtn.setAttribute("id", `remove-${type}-btn-${i + 1}`);
        removeBtn.textContent = "Remove";

        buttonDiv.appendChild(removeBtn);


        let formGroup1 = document.createElement("div");
        formGroup1.setAttribute("class", "form-group");

        let formGroup2 = document.createElement("div");
        formGroup2.setAttribute("class", "form-group");

        let nameLabel = document.createElement("label");
        nameLabel.setAttribute("for", `${Type}-name-${i + 1}`);
        nameLabel.setAttribute("class", "control-label");
        nameLabel.textContent = "Name";

        let nameInput = document.createElement("input");
        nameInput.setAttribute("type", "text");
        nameInput.setAttribute("class", "form-control");
        nameInput.setAttribute("id", `${Type}-name-${i + 1}`);
        nameInput.setAttribute("data-val", true);
        nameInput.setAttribute("data-val-required", "A Name is required");
        nameInput.setAttribute("name", `New${Type}Locations[${i}].Name`);
        if (type === "food") {
            nameInput.setAttribute("placeholder", "Name of the Food or Eating Location")
        } else {
            nameInput.setAttribute("placeholder", "Name of the Place you'd like to visit")
        }


        let nameValidator = document.createElement("span");
        nameValidator.setAttribute("class", "text-danger field-validation-valid");
        nameValidator.setAttribute("data-valmsg-for", `New${Type}Locations[${i}].Name`);
        nameValidator.setAttribute("data-valmsg-replace", true);

        let descLabel = document.createElement("label");
        descLabel.setAttribute("for", `${Type}-desc-${i + 1}`);
        descLabel.setAttribute("class", "control-label");
        descLabel.textContent = "Description";

        let descTextarea = document.createElement("textarea");
        descTextarea.setAttribute("type", "text");
        descTextarea.setAttribute("class", "form-control");
        descTextarea.setAttribute("id", `${Type}-desc-${i + 1}`);
        descTextarea.setAttribute("name", `New${Type}Locations[${i}].Description`);
        if (type === "food") {
            descTextarea.setAttribute("placeholder", "(optional) Describe the Food or Eating Location")
        } else {
            descTextarea.setAttribute("placeholder", "(optional) Describe the Place you'd like to visit")
        }

        formGroup1.appendChild(nameLabel);
        formGroup1.appendChild(nameInput);
        formGroup1.appendChild(nameValidator);

        formGroup2.appendChild(descLabel);
        formGroup2.appendChild(descTextarea);

        innerDiv.appendChild(formGroup1);
        innerDiv.appendChild(formGroup2);

        newParent.appendChild(innerDiv);
        newParent.appendChild(buttonDiv);

        container.appendChild(newParent);

        if (type === "food") {
            return foodI++
        } else {
            return visitI++
        }

    }

    if (e.target.id.includes("remove")) {

        let type = e.target.id.split("-")[1];
        let Type = type.charAt(0).toUpperCase() + type.slice(1);

        // grab the aread to be removed and the container its being removed from
        let removeDiv = e.target.parentElement.parentElement;
        let container = removeDiv.parentElement;

        // remove the element
        container.removeChild(removeDiv);

        // fix the incrementer
        let i = container.children.length;

        for (let j = 0; j < i; j++) {
            let parent = container.children[j]
            parent.setAttribute("id", `${type}-location-${j + 1}`)
            let nameLabel = container.children[j].children[0].children[0].children[0]
            let nameInput = container.children[j].children[0].children[0].children[1]
            let nameValidator = container.children[j].children[0].children[0].children[2]
            nameLabel.setAttribute("for", `${Type}-name-${j + 1}`)
            nameInput.setAttribute("name", `New${Type}Locations[${j}].Name`)
            nameInput.setAttribute("id", `${Type}-name-${j + 1}`)
            nameValidator.setAttribute("data-valmsg-for", `NewFoodLocations[${j}].Name`)
            let descLabel = container.children[j].children[0].children[1].children[0]
            let descTextarea = container.children[j].children[0].children[1].children[1]
            descLabel.setAttribute("for", `${Type}-desc-${j + 1}`)
            descTextarea.setAttribute("name", `New${Type}Locations[${j}].Description`)
            descTextarea.setAttribute("id", `${Type}-desc-${j + 1}`)
            let removeBtn = container.children[j].children[1].children[0]
            removeBtn.setAttribute("id", `remove-${type}-btn-${j + 1}`)
        }

        if (type === "food") {
            foodI = i;
            return foodI
        } else if (type === "visit") {
            visitI = i;
            return visitI
        }
    }
})