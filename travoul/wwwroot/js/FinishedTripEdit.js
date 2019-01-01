
console.log("Dope");


const parentDiv = document.querySelector("#all-locations-container");

parentDiv.addEventListener("click", (e) => {

    if (e.target.id.includes("remove")) {
        e.preventDefault();
        console.log(e.target);

        let type = e.target.id.split("-")[1];
        let Type = type.charAt(0).toUpperCase() + type.slice(1);
        let container = document.querySelector(`#${type}-container`);
        //let i = container.children.length - 1;

        console.log(type);
        console.log(Type);
        console.log(container);
        //console.log(i);

        container.removeChild(e.target.parentElement);

        for (let i = 0; i < container.children.length; i++) {

            //First form groups inputs
            let nameLabel = container.children[i].children[0].children[0];
            let nameInput = container.children[i].children[0].children[1];
            let nameValidator = container.children[i].children[0].children[2];
            nameLabel.setAttribute("for", `New${Type}Locations_${i}__Name`);
            nameInput.setAttribute("id", `${type}-name-${i + 1}`);
            nameInput.setAttribute("name", `New${Type}Locations[${i}].Name`);
            nameValidator.setAttribute("data-valmsg-for", `New${Type}Locations[${i}].Name`);

            //Second form groups inputs
            let descLabel = container.children[i].children[1].children[0];
            let descInput = container.children[i].children[1].children[1];
            let descValidator = container.children[i].children[1].children[2];
            descLabel.setAttribute("for", `New${Type}Locations_${i}__Description`);
            descInput.setAttribute("id", `${type}-desc-${i + 1}`);
            descInput.setAttribute("name", `New${Type}Locations[${i}].Description`);
            descValidator.setAttribute("data-valmsg-for", `New${Type}Locations[${i}].Description`);

            //third form group, the checkbox
            let checkbox = container.children[i].children[2].children[0].children[0].children[0];
            checkbox.setAttribute("name", `New${Type}Locations[${i}].IsCompleted`);
            checkbox.setAttribute("id", `New${Type}Locations_${i}__IsCompleted`);

            //fourth child, the remove button
            let removeBtn = container.children[i].children[3];
            removeBtn.setAttribute("id", `remove-${type}-btn-${i + 1}`);

            //fifth, fix parent id
            let parent = container.children[i];
            parent.setAttribute("id", `${type}-location-${i + 1}`);

        }



    }

    if (e.target.id === "add-food-btn" || e.target.id === "add-visit-btn") {
        e.preventDefault();

        let type = e.target.id.split("-")[1];
        let Type = type.charAt(0).toUpperCase() + type.slice(1);
        let parent = document.querySelector(`#${type}-container`);
        let i = parent.children.length;

        let formGroup = document.createElement("div");
        formGroup.setAttribute("class", "form-group");

        let checkbox = document.createElement("div");
        checkbox.setAttribute("class", "checkbox");
        let label = document.createElement("label");
        let inputCheckbox = document.createElement("input");
        inputCheckbox.type = "checkbox";
        inputCheckbox.setAttribute("data-val", true);
        inputCheckbox.setAttribute("id", `New${Type}Locations_${i}__IsCompleted`);
        inputCheckbox.setAttribute("name", `New${Type}Locations[${i}].IsCompleted`);
        inputCheckbox.setAttribute("value", true);
        inputCheckbox.setAttribute("checked", "checked");
        let text = document.createTextNode("Completed?");

        label.appendChild(inputCheckbox);
        label.appendChild(text);
        checkbox.appendChild(label);
        formGroup.appendChild(checkbox);


        let newLocationNameGroup = document.createElement("div");
        newLocationNameGroup.setAttribute("class", "form-group");

        let newLocationDescGroup = document.createElement("div");
        newLocationDescGroup.setAttribute("class", "form-group");


        //< span class="text-danger field-validation-valid" data - valmsg -for= "NewFoodLocations[5].Name" data - valmsg - replace= "true" ></span >

        let nameInput = document.createElement("input");
        let nameLabel = document.createElement("label");
        let nameValidator = document.createElement("span");
        nameLabel.textContent = "Name";
        nameLabel.setAttribute("for", `New${Type}Locations_${i}__Name`);
        nameLabel.setAttribute("class", "control-label");
        //nameInput.setAttribute("Placeholder", "Ex. Hattie B's")
        nameInput.setAttribute("id", `New${Type}Locations_${i}__Name`);
        nameInput.setAttribute("class", "form-control");
        nameInput.setAttribute("data-val", true);
        nameInput.setAttribute("data-val-required", "The Name field is required");
        nameInput.setAttribute("name", `New${Type}Locations[${i}].Name`);
        nameValidator.setAttribute("class", "text-danger field-validation-valid");
        nameValidator.setAttribute("data-valmsg-for", `New${Type}Locations[${i}].Name`);
        nameValidator.setAttribute("data-valmsg-replace", true);

        let descriptionInput = document.createElement("input");
        let descriptionLabel = document.createElement("label");
        let descValidator = document.createElement("span");
        descriptionLabel.setAttribute("for", `New${Type}Locations_${i}__Description`);
        descriptionLabel.textContent = "Description";
        descriptionInput.setAttribute("class", "control-label");
        //descriptionInput.setAttribute("Placeholder", "Ex. They have great hot chicken")
        descriptionInput.setAttribute("class", "form-control");
        descriptionInput.setAttribute("id", `New${Type}Locations_${i}__Description`);
        descriptionInput.setAttribute("data-val", true);
        descriptionInput.setAttribute("name", `New${Type}Locations[${i}].Description`);
        descValidator.setAttribute("class", "text-danger field-validation-valid");
        descValidator.setAttribute("data-valmsg-for", `New${Type}Locations[${i}].Description`);
        descValidator.setAttribute("data-valmsg-replace", true);

        newLocationNameGroup.appendChild(nameLabel);
        newLocationNameGroup.appendChild(nameInput);
        newLocationNameGroup.appendChild(nameValidator);

        newLocationDescGroup.appendChild(descriptionLabel);
        newLocationDescGroup.appendChild(descriptionInput);
        newLocationDescGroup.appendChild(descValidator);

        let removeBtn = document.createElement("button");
        removeBtn.setAttribute("class", "btn btn-sm btn-danger");
        removeBtn.setAttribute("id", `remove-${type}-btn-${i + 1}`);
        removeBtn.textContent = "Remove";

        let newDiv = document.createElement("div");
        newDiv.setAttribute("id", `${type}-location-${i + 1}`);
        newDiv.appendChild(newLocationNameGroup);
        newDiv.appendChild(newLocationDescGroup);
        newDiv.appendChild(formGroup);
        newDiv.appendChild(removeBtn);
        parent.appendChild(newDiv);
    }
})
