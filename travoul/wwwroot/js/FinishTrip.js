
console.log("Dope");


let foodIndex = 0;

let placeIndex = 0;

const parentDiv = document.querySelector(".retros-div");

const foodButton = document.querySelector("#add-food-btn");
const placeButton = document.querySelector("#add-place-btn");

const newFoodDiv = document.querySelector("#new-food-container");
const newPlaceDiv = document.querySelector("#new-place-container");

parentDiv.addEventListener("click", (e) => {

    if (e.target.id === "add-food-btn" || e.target.id === "add-place-btn") {
        e.preventDefault();

        let index = 0;

        let type = e.target.id.split("-")[1];
        let Type = type.charAt(0).toUpperCase() + type.slice(1);

        console.log(`${type} btn clicked`)

        if (type === "food") {
            index = foodIndex
        } else {
            index = placeIndex
        }

        let formGroup = document.createElement("div");
        formGroup.setAttribute("class", "form-group");

        let checkbox = document.createElement("div");
        checkbox.setAttribute("class", "checkbox");
        let label = document.createElement("label");
        let inputCheckbox = document.createElement("input");
        inputCheckbox.type = "checkbox";
        inputCheckbox.setAttribute("checked", "checked")
        inputCheckbox.setAttribute("value", true);
        inputCheckbox.setAttribute("data-val", true)
        inputCheckbox.setAttribute("name", `New${Type}s[${index}].IsCompleted`)
        let text = document.createTextNode("Completed")



        label.appendChild(inputCheckbox);
        label.appendChild(text);
        checkbox.appendChild(label);
        formGroup.appendChild(checkbox);


        let newLocationGroup = document.createElement("div")
        newLocationGroup.setAttribute("class", "form-group half-width")
        let nameInput = document.createElement("input")
        let nameLabel = document.createElement("label")
        nameLabel.textContent = "Name"
        nameLabel.setAttribute("for", `${Type}-name-${index + 1}`)
        nameLabel.setAttribute("class", "control-label")
        nameInput.setAttribute("Placeholder", "Ex. Hattie B's")
        nameInput.setAttribute("id", `${Type}-name-${index + 1}`)
        nameInput.setAttribute("class", "form-control")
        nameInput.setAttribute("data-val", true)
        nameInput.setAttribute("data-val-required", "The Name field is required")
        nameInput.setAttribute("name", `New${Type}s[${index}].Name`)

        let descriptionInput = document.createElement("input")
        let descriptionLabel = document.createElement("label")
        descriptionLabel.setAttribute("for", `${Type}-desc-${index + 1}`)
        descriptionLabel.textContent = "Description "
        descriptionInput.setAttribute("class", "control-label")
        descriptionInput.setAttribute("Placeholder", "Ex. They have great hot chicken")
        descriptionInput.setAttribute("class", "form-control")
        descriptionInput.setAttribute("id", `${Type}-desc-${index + 1}`)
        descriptionInput.setAttribute("data-val", true)
        descriptionInput.setAttribute("name", `New${Type}s[${index}].Description`)

        newLocationGroup.appendChild(nameLabel)
        newLocationGroup.appendChild(nameInput)
        newLocationGroup.appendChild(descriptionLabel)
        newLocationGroup.appendChild(descriptionInput)


        if (type === "food") {
            newFoodDiv.appendChild(newLocationGroup)
            newFoodDiv.appendChild(formGroup)
            return foodIndex++
        } else {
            newPlaceDiv.appendChild(newLocationGroup)
            newPlaceDiv.appendChild(formGroup)
            return placeIndex++
        }

    }

})
