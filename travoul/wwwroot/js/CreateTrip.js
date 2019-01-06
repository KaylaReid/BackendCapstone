

let locationInput = document.querySelector("#Trip_Location");


locationInput.addEventListener("keyup", (e) => {

    let locationText = e.target.value;

    let googleQuery = "https://www.google.com/search?q=things+to+do+in+";

    let newQuery = googleQuery + locationText.split(" ").join("+");

    document.querySelector("#search-activity-btn").setAttribute("href", newQuery);
})




//set incrementers for the food places and visit places
let foodI = 1
let visitI = 1


//add event listener to the visit locations container
document.querySelector(".plan-create-locations-card").addEventListener("click", (e) => {


    //perform actions when add food/add visit buttons are clicked
    if (e.target.id === "add-food-btn" || e.target.id === "add-visit-btn") {
    //prevent overall form submission
    e.preventDefault();

        //grab type (food or visit) from button ids
        let type = e.target.id.split("-")[1];

        //create capitalized version for interpolation purposes
        let Type = type.charAt(0).toUpperCase() + type.slice(1);

        //instantiate an indexer
        let i = 0;

        //set indexer to equal foodI or visitI depending on type
        if (type === "food") {
            i = foodI
        } else {
            i = visitI
        }

        //grab container for food or visit
        let container = document.querySelector(`.${type}-container`);

        //create new card for new visit location and set attributes
        let newParent = document.createElement("div");
        newParent.setAttribute("class", "card p-2 bg-light mb-2");
        newParent.setAttribute("id", `${type}-location-${i + 1}`);

        //create innerdiv to mirror what is on page currently
        let innerDiv = document.createElement("div");

        //create form-group divs to house the inputs
        let formGroup1 = document.createElement("div");
        formGroup1.setAttribute("class", "form-group");

        let formGroup2 = document.createElement("div");
        formGroup2.setAttribute("class", "form-group");

        //build out name area, including a label, input, and span for validation
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
        nameInput.setAttribute("name", `EnteredTrip${Type}Locations[${i}].Name`);

        let nameValidator = document.createElement("span");
        nameValidator.setAttribute("class", "text-danger field-validation-valid");
        nameValidator.setAttribute("data-valmsg-for", `EnteredTrip${Type}Locations[${i}].Name`);
        nameValidator.setAttribute("data-valmsg-replace", true);


        //build out description area with label and textarea
        let descLabel = document.createElement("label");
        descLabel.setAttribute("for", `${Type}-desc-${i + 1}`);
        descLabel.setAttribute("class", "control-label");
        descLabel.textContent = "Description";

        let descTextarea = document.createElement("textarea");
        descTextarea.setAttribute("type", "text");
        descTextarea.setAttribute("class", "form-control");
        descTextarea.setAttribute("id", `${Type}-desc-${i + 1}`);
        descTextarea.setAttribute("name", `EnteredTrip${Type}Locations[${i}].Description`);


        //place name items in first form group and description items in second form group
        formGroup1.appendChild(nameLabel);
        formGroup1.appendChild(nameInput);
        formGroup1.appendChild(nameValidator);

        formGroup2.appendChild(descLabel);
        formGroup2.appendChild(descTextarea);

        //place form groups into innerdiv
        innerDiv.appendChild(formGroup1);
        innerDiv.appendChild(formGroup2);

        //put innerdiv into the new card
        newParent.appendChild(innerDiv);

        //insert new card onto DOM in appropriate container
        container.appendChild(newParent);

        //return updated incrementer for future adds accuracy.
        if (type === "food") {
            return foodI++
        } else {
            return visitI++
        }
    }
})
