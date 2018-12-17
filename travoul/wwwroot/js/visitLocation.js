//for FoodPlaces

let foodIncrementer = 4;

const selectFood = document.querySelector(".add-more-food").addEventListener("click", e => {

    let newFoodDiv = document.createDocumentFragment()
    let nameInput = document.createElement("input")
    let nameLabel = document.createElement("label")
    nameLabel.textContent = "Name"
    nameLabel.setAttribute("for", `Food-name-${foodIncrementer}`)
    nameInput.setAttribute("Placeholder", "Ex. Hattie B's")
    nameInput.setAttribute("id", `Food-name-${foodIncrementer}`)
    nameInput.setAttribute("data-val", true)
    nameInput.setAttribute("data-val-required", "The Name field is required")
    nameInput.setAttribute("name", `EnteredTripFoodLocations[${foodIncrementer - 1}].Name`)

    let descriptionInput = document.createElement("input")
    let descriptionLabel = document.createElement("label")
    descriptionLabel.setAttribute("for", `Food-desc-${foodIncrementer}`)
    descriptionLabel.textContent = "Description "
    descriptionInput.setAttribute("Placeholder", "Ex. They have great hot chicken")
    descriptionInput.setAttribute("id", `Food-desc-${foodIncrementer}`)
    descriptionInput.setAttribute("data-val", true)
    descriptionInput.setAttribute("name", `EnteredTripFoodLocations[${foodIncrementer - 1}].Description`)

    newFoodDiv.appendChild(nameLabel)
    newFoodDiv.appendChild(nameInput)
    newFoodDiv.appendChild(document.createElement("br"))
    newFoodDiv.appendChild(descriptionLabel)
    newFoodDiv.appendChild(descriptionInput)
    newFoodDiv.appendChild(document.createElement("br"))
    document.querySelector(".Food").appendChild(newFoodDiv)

    return foodIncrementer++
})


// for places 
let placeIncrementer = 4;

const selectPlaces = document.querySelector(".add-more-places").addEventListener("click", e => {

    //builds up name input for places
    let newPlacesDiv = document.createDocumentFragment()
    let nameInput = document.createElement("input")
    let nameLabel = document.createElement("label")
    nameLabel.textContent = "Name "
    nameLabel.setAttribute("for", `Place-name-${placeIncrementer}`)
    nameInput.setAttribute("Placeholder", "Ex. The Frist")
    nameInput.setAttribute("id", `Place-name-${placeIncrementer}`)
    nameInput.setAttribute("data-val", true)
    nameInput.setAttribute("data-val-required", "The Name field is required")
    nameInput.setAttribute("name", `EnteredTripVisitLocations[${placeIncrementer - 1}].Name`)

    //builds up desc input for places
    let descriptionInput = document.createElement("input")
    let descriptionLabel = document.createElement("label")
    descriptionLabel.setAttribute("for", `Place-desc-${placeIncrementer}`)
    descriptionLabel.textContent = "Description "
    descriptionInput.setAttribute("Placeholder", "Ex. Art Museum")
    descriptionInput.setAttribute("id", `Place-desc-${placeIncrementer}`)
    descriptionInput.setAttribute("data-val", true)
    descriptionInput.setAttribute("name", `EnteredTripVisitLocations[${placeIncrementer - 1}].Description`)

    newPlacesDiv.appendChild(nameLabel)
    newPlacesDiv.appendChild(nameInput)
    newPlacesDiv.appendChild(document.createElement("br"))
    newPlacesDiv.appendChild(descriptionLabel)
    newPlacesDiv.appendChild(descriptionInput)
    newPlacesDiv.appendChild(document.createElement("br"))
    document.querySelector(".Places").appendChild(newPlacesDiv)

    return placeIncrementer++
})


//fetch("http://localhost:5000/Trips/Create")

//    .then(res => res.json())
//    .then(songs => {

//        const songOption = document.createElement("option")
//        songOption.value = "0"
//        songOption.textContent = "Select a favorite song..."
//        select.appendChild(songOption)

//        songs.map(song => {
//            const songOption = document.createElement("option")
//            songOption.value = song.songId
//            songOption.textContent = song.title
//            select.appendChild(songOption)
//        })
//    })