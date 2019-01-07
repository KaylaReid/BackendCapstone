
// get current img url
const imgSrc = document.querySelector("#trip-img").getAttribute("src");

// get buttons for manipulating img
let previewBtn = document.querySelector("#img-preview-btn");
let undoBtn = document.querySelector("#img-undo-btn");

// event listener on preview button to change img
previewBtn.addEventListener("click", (e) => {
    // prevent form submission
    e.preventDefault();

    // get new value from input
    let newSrc = document.querySelector("#new-img-link").value;

    // set img src to = value, so user can see img 
    document.querySelector("#trip-img").setAttribute("src", newSrc);

})

// event listener on undo btn to change img back to what it was
undoBtn.addEventListener("click", (e) => {
    // prevent form submission
    e.preventDefault();

    // set img back to original src
    document.querySelector("#trip-img").setAttribute("src", imgSrc);

    // fill input with original src for form submission
    document.querySelector("#new-img-link").value = imgSrc;

})

// grab title input for polaroid writing
let titleInput = document.querySelector("#Trip_Title");

// event listener for changing sharpie writing on polaroid to match user input
titleInput.addEventListener("keyup", (e) => {
    // grab new value after each keypress
    let titleText = e.target.value;

    // put value on polaroid in sharpie
    document.querySelector("#polaroid-img-title").textContent = titleText;
})

// grab locations container for adding and deleting trip visit locations
const parentDiv = document.querySelector("#all-locations-container");

// event listener to handle adding and removing trip visit locations
parentDiv.addEventListener("click", (e) => {


    // Event listener for checkbox to handle class changes
    if (e.target.id.includes("IsCompleted")) {

        //set type based on id
        let type = "";

        if (e.target.id.includes("Food")) {
            type = "food";
        } else {
            type = "visit";
        }

        // get num from id 'NewTypeLocations_num__IsCompleted'
        let num = parseInt(e.target.id.split("__")[0].split("_")[1]) + 1;

        // get parent from type and num
        let parent = document.querySelector(`#${type}-location-${num}`)

        // comes back backwards, so if true remove incomplete classes and add complete, if false do the reverse 
        if (e.target.checked === true) {
            // determine if orange or green
            if (type === "food") {
                parent.classList.remove("inset-shadow-orange");
            } else {
                parent.classList.remove("inset-shadow-green");
            }
            parent.classList.remove("bg-dark");
            parent.classList.remove("border-0");
            parent.classList.remove("text-light");
            parent.classList.add("bg-light");
            parent.classList.add("inset-shadow");
        } else {
            parent.classList.remove("bg-light");
            parent.classList.remove("inset-shadow");
            if (type === "food") {
                parent.classList.add("inset-shadow-orange");
            } else {
                parent.classList.add("inset-shadow-green");
            }
            parent.classList.add("bg-dark");
            parent.classList.add("border-0");
            parent.classList.add("text-light");
        }


    }

    // resets all ids and data attributes when a location is removed from the DOM
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

    // builds a new card and loads in all relevent data attributes for adding a location
    if (e.target.id === "add-food-btn" || e.target.id === "add-visit-btn") {
        //prevent form submission
        e.preventDefault();

        // get variable for interpolation and DOM appending
        let type = e.target.id.split("-")[1];
        let Type = type.charAt(0).toUpperCase() + type.slice(1);
        let parent = document.querySelector(`.${type}-container`);
        let i = parent.children.length;

        // create new card for new location
        let newCard = document.createElement("div");
        newCard.setAttribute("class", "card p-2 mt-3 mb-2 bg-light inset-shadow");
        newCard.setAttribute("id", `${type}-location-${i + 1}`);

        // build out the next two divs
        let innerDiv1 = document.createElement("div");
        innerDiv1.setAttribute("class", "d-flex justify-content-start");

        let innerDiv2 = document.createElement("div");
        innerDiv2.setAttribute("class", "d-flex justify-content-end");

        // make remove btn and append to second div
        let removeBtn = document.createElement("button");
        removeBtn.setAttribute("class", "btn btn-sm btn-secondary btn-delete");
        removeBtn.setAttribute("id", `remove-${type}-btn-${i + 1}`);
        removeBtn.textContent = "Remove";

        innerDiv2.appendChild(removeBtn);

        // make all elements associated with the checkbox
        let checkboxDiv = document.createElement("div");
        checkboxDiv.setAttribute("class", "checkbox width-30 mr-2 ml-2 d-flex flex-column justify-content-center");

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

        // append checkbox to first div
        innerDiv1.appendChild(checkboxDiv);

        // create div to hold the inputs
        let inputsDiv = document.createElement("div");
        inputsDiv.setAttribute("class", "width-65");

        // build two form group divs to mirrow other cards
        let formGroup1 = document.createElement("div");
        formGroup1.setAttribute("class", "form-group");

        let formGroup2 = document.createElement("div");
        formGroup2.setAttribute("class", "form-group");

        // name name input
        let nameInput = document.createElement("input");
        nameInput.setAttribute("class", "form-control");
        nameInput.setAttribute("placeholder", "Name");
        nameInput.setAttribute("id", `${type}-name-${i + 1}`);
        nameInput.setAttribute("type", "text");
        nameInput.setAttribute("data-val", true);
        nameInput.setAttribute("data-val-required", "The Name field is required.");
        nameInput.setAttribute("name", `New${Type}Locations[${i}].Name`);

        // span for submission validation
        let nameValidator = document.createElement("span");
        nameValidator.setAttribute("class", "text-danger field-validation-valid");
        nameValidator.setAttribute("data-valmsg-for", `New${Type}Locations[${i}].Name`);
        nameValidator.setAttribute("data-valmsg-replace", true);

        // place into first form group
        formGroup1.appendChild(nameInput);
        formGroup1.appendChild(nameValidator);

        // make textarea
        let descTextarea = document.createElement("textarea");
        descTextarea.setAttribute("name", `New${Type}Locations[${i}].Description`);
        descTextarea.setAttribute("class", "form-control");
        descTextarea.setAttribute("placeholder", "Description");
        descTextarea.setAttribute("id", `${type}-desc-${i + 1}`);

        // place in second form group
        formGroup2.appendChild(descTextarea);

        // put the name and text areas into the inputs div, and append that after the checkbox into the innerDiv1
        inputsDiv.appendChild(formGroup1);
        inputsDiv.appendChild(formGroup2);

        innerDiv1.appendChild(inputsDiv);

        // place innerdivs into newCard
        newCard.appendChild(innerDiv1)
        newCard.appendChild(innerDiv2)

        // put new card on the DOM
        parent.appendChild(newCard);

    }
})