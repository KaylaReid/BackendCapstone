
console.log("Dope");

const imgSrc = document.querySelector("#trip-img").getAttribute("src");

let previewBtn = document.querySelector("#img-preview-btn");
let undoBtn = document.querySelector("#img-undo-btn");

previewBtn.addEventListener("click", (e) => {
    e.preventDefault();

    let newSrc = document.querySelector("#new-img-link").value;

    document.querySelector("#trip-img").setAttribute("src", newSrc);

})

undoBtn.addEventListener("click", (e) => {
    e.preventDefault();

    document.querySelector("#trip-img").setAttribute("src", imgSrc);

    document.querySelector("#new-img-link").value = imgSrc;


})






const parentDiv = document.querySelector("#all-locations-container");

parentDiv.addEventListener("click", (e) => {

    if (e.target.id.includes("remove")) {
        e.preventDefault();

        let type = e.target.id.split("-")[1];
        let Type = type.charAt(0).toUpperCase() + type.slice(1);
        let container = document.querySelector(`.${type}-container`);
        let num = e.target.id.split("-")[3]
        let parent = document.querySelector(`#${type}-location-${num}`);

        container.removeChild(parent);

        for (let i = 0; i < container.children.length; i++) {


            //first section, the checkbox
            let checkbox = container.children[i].children[0].children[0].children[0].children[0];
            checkbox.setAttribute("name", `New${Type}Locations[${i}].IsCompleted`);
            checkbox.setAttribute("id", `New${Type}Locations_${i}__IsCompleted`);

            //second section,  name input group
            let nameInput = container.children[i].children[0].children[1].children[0].children[0];
            let nameValidator = container.children[i].children[0].children[1].children[0].children[1];
            nameInput.setAttribute("id", `${type}-name-${i + 1}`);
            nameInput.setAttribute("name", `New${Type}Locations[${i}].Name`);
            nameValidator.setAttribute("data-valmsg-for", `New${Type}Locations[${i}].Name`);

            //third section,  textarea
            let descTextarea = container.children[i].children[0].children[1].children[1].children[0];
            descTextarea.setAttribute("id", `${type}-desc-${i + 1}`);
            descTextarea.setAttribute("name", `New${Type}Locations[${i}].Description`);


            //fourth, the remove button
            let removeBtn = container.children[i].children[1].children[0];
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
        let parent = document.querySelector(`.${type}-container`);
        let i = parent.children.length;


        let newCard = document.createElement("div");
        newCard.setAttribute("class", "card p-2 bg-light mt-3 mb-2");
        newCard.setAttribute("id", `${type}-location-${i + 1}`);

        let innerDiv1 = document.createElement("div");
        innerDiv1.setAttribute("class", "d-flex justify-content-start");

        let innerDiv2 = document.createElement("div");
        innerDiv2.setAttribute("class", "d-flex justify-content-end");

        let removeBtn = document.createElement("button");
        removeBtn.setAttribute("class", "btn btn-sm btn-danger");
        removeBtn.setAttribute("id", `remove-${type}-btn-${i + 1}`);
        removeBtn.textContent = "Remove";

        innerDiv2.appendChild(removeBtn);

        let checkboxDiv = document.createElement("div");
        checkboxDiv.setAttribute("class", "checkbox width-30 mr-2 d-flex flex-column justify-content-center");

        let label = document.createElement("label");
        let checkbox = document.createElement("input");
        checkbox.type = "checkbox";
        checkbox.setAttribute("checked", "checked");
        checkbox.setAttribute("data-val", true)
        checkbox.setAttribute("value", true)
        checkbox.setAttribute("id", `New${Type}Locations_${i}__IsCompleted`)
        checkbox.setAttribute("name", `New${Type}Locations[${i}].IsCompleted`)

        let text = document.createTextNode("Completed?");


        label.appendChild(checkbox);
        label.appendChild(text);

        checkboxDiv.appendChild(label);

        innerDiv1.appendChild(checkboxDiv);

        let inputsDiv = document.createElement("div");
        inputsDiv.setAttribute("class", "width-65");

        let formGroup1 = document.createElement("div");
        formGroup1.setAttribute("class", "form-group");

        let formGroup2 = document.createElement("div");
        formGroup2.setAttribute("class", "form-group");

        let nameInput = document.createElement("input");
        nameInput.setAttribute("class", "form-control");
        nameInput.setAttribute("placeholder", "Name");
        nameInput.setAttribute("id", `${type}-name-${i + 1}`);
        nameInput.setAttribute("type", "text");
        nameInput.setAttribute("data-val", true);
        nameInput.setAttribute("data-val-required", "The Name field is required.");
        nameInput.setAttribute("name", `New${Type}Locations[${i}].Name`);

        let nameValidator = document.createElement("span");
        nameValidator.setAttribute("class", "text-danger field-validation-valid");
        nameValidator.setAttribute("data-valmsg-for", `New${Type}Locations[${i}].Name`);
        nameValidator.setAttribute("data-valmsg-replace", true);

        formGroup1.appendChild(nameInput);
        formGroup1.appendChild(nameValidator);

        let descTextarea = document.createElement("textarea");
        descTextarea.setAttribute("name", `New${Type}Locations[${i}].Description`);
        descTextarea.setAttribute("class", "form-control");
        descTextarea.setAttribute("placeholder", "Description");
        descTextarea.setAttribute("id", `${type}-desc-${i + 1}`);

        formGroup2.appendChild(descTextarea);

        inputsDiv.appendChild(formGroup1);
        inputsDiv.appendChild(formGroup2);

        innerDiv1.appendChild(inputsDiv);

        newCard.appendChild(innerDiv1)
        newCard.appendChild(innerDiv2)


        parent.appendChild(newCard);

    }
})
