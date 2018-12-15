//for FoodPlaces
const selectFood = document.querySelector(".add-more-food").addEventListener("click", e => {


    let newFoodDiv = document.createDocumentFragment()
    let nameInput = document.createElement("input")
    let nameLabel = document.createElement("label")
    nameLabel.textContent = "Name "

    let desctiptionInput = document.createElement("input")
    let descriptionLabel = document.createElement("label")
    descriptionLabel.textContent = "Description "

    newFoodDiv.appendChild(nameLabel)
    newFoodDiv.appendChild(nameInput)
    newFoodDiv.appendChild(document.createElement("br"))
    newFoodDiv.appendChild(descriptionLabel)
    newFoodDiv.appendChild(desctiptionInput)
    newFoodDiv.appendChild(document.createElement("br"))
    document.querySelector(".food").appendChild(newFoodDiv)
})

let getFoodValues = () => {
    console.log("Dope food")
}

// for places 
const selectPlaces = document.querySelector(".add-more-places").addEventListener("click", e => {


    let newPlacesDiv = document.createDocumentFragment()
    let nameInput = document.createElement("input")
    let nameLabel = document.createElement("label")
    nameLabel.textContent = "Name "

    let desctiptionInput = document.createElement("input")
    let descriptionLabel = document.createElement("label")
    descriptionLabel.textContent = "Description "

    newPlacesDiv.appendChild(nameLabel)
    newPlacesDiv.appendChild(nameInput)
    newPlacesDiv.appendChild(document.createElement("br"))
    newPlacesDiv.appendChild(descriptionLabel)
    newPlacesDiv.appendChild(desctiptionInput)
    newPlacesDiv.appendChild(document.createElement("br"))
    document.querySelector(".places").appendChild(newPlacesDiv)
})

let getPlacesValues = () => {
    console.log("Dope place")
}

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