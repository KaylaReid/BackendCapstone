console.log("Dope")

const locationContainer = document.querySelector("#all-locations-container");

let foodIncrementer = document.querySelector("#food-container").children.length;
console.log(foodIncrementer)
let visitIncrementer = document.querySelector("#visit-container").children.length;
console.log(visitIncrementer)

locationContainer.addEventListener("click", (e) => {
    e.preventDefault();
    if (e.target.id === "add-food-btn" || e.target.id === "add-visit-btn")
    {
        let i = 0;

        let type = e.target.id.split("-")[1]

        let bigType = type.charAt(0).toUpperCase() + type.slice(1);

        if (type === "food") {
            foodIncrementer++
            i = foodIncrementer
        } else if (type === "visit") {
            visitIncrementer++
            i = visitIncrementer
        }


        //create parent div for new inputs, give it the proper Id
        let newDiv = document.createElement("div")
        newDiv.setAttribute("id", `${type}-location-${i}`)

        //create form group containers for the input and label
        let nameFormGroup = document.createElement("div")
        nameFormGroup.setAttribute("class", "form-group")
        let descFromGroup = document.createElement("div")
        descFromGroup.setAttribute("class", "form-group")

        //create input and label for name and give it the required attributes
        let nameInput = document.createElement("input")
        let nameLabel = document.createElement("label")
        nameLabel.textContent = "Name"
        nameLabel.setAttribute("for", `${type}-name-${i}`)
        nameLabel.setAttribute("class", "control-label")
        nameInput.setAttribute("Placeholder", "Ex. Hattie B's")
        nameInput.setAttribute("class", "form-control")
        nameInput.setAttribute("id", `${type}-name-${i}`)
        nameInput.setAttribute("data-val", true)
        nameInput.setAttribute("data-val-required", "The Name field is required")
        nameInput.setAttribute("name", `New${bigType}Locations[${i - 1}].Name`)

        // create input and label for description and give it required attributes
        let descriptionInput = document.createElement("input")
        let descriptionLabel = document.createElement("label")
        descriptionLabel.setAttribute("for", `${type}-desc-${i}`)
        descriptionLabel.setAttribute("class", "control-label")
        descriptionLabel.textContent = "Description"
        descriptionInput.setAttribute("Placeholder", "Ex. They have great hot chicken")
        descriptionInput.setAttribute("class", "form-control")
        descriptionInput.setAttribute("id", `${type}-desc-${i}`)
        descriptionInput.setAttribute("data-val", true)
        descriptionInput.setAttribute("name", `New${bigType}Locations[${i - 1}].Description`)

        // place name elements into form group and append to parent container 
        nameFormGroup.appendChild(nameLabel)
        nameFormGroup.appendChild(nameInput)
        newDiv.appendChild(nameFormGroup)

        descFromGroup.appendChild(descriptionLabel)
        descFromGroup.appendChild(descriptionInput)
        newDiv.appendChild(descFromGroup)

        // add a remove button
        let removeBtn = document.createElement("button")
        removeBtn.setAttribute("class", `btn btn-sm ${type}-btn-${i}`)
        removeBtn.textContent = "Remove"
        newDiv.appendChild(removeBtn)

        document.querySelector(`#${type}-container`).appendChild(newDiv)

        if (type === "food") {
            return foodIncrementer
        } else if (type === "visit") {
            return visitIncrementer
        }

    }
})