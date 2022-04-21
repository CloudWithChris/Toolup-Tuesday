package controllers

import (
	"net/http"
	"time"

	"github.com/cloudwithchris/toolup-tuesday/src/SpaceBar.PlayerDecisions/models"
	"github.com/gin-gonic/gin"
)

var playerDecisions = []models.PlayerDecision{
	{DecisionID: "1", PlayerID: "1", PlayerName: "John", Decision: "Yes", WorldEventID: "12345", DecisionDate: time.Now().String()},
	{DecisionID: "2", PlayerID: "2", PlayerName: "Margaret", Decision: "No", WorldEventID: "12345", DecisionDate: time.Now().String()},
	{DecisionID: "3", PlayerID: "3", PlayerName: "Bob", Decision: "Yes", WorldEventID: "12345", DecisionDate: time.Now().String()},
}

// getAlbums responds with the list of all albums as JSON.
func GetDecisions(c *gin.Context) {
	c.IndentedJSON(http.StatusOK, playerDecisions)
}

// postAlbums adds an album from JSON received in the request body.
func PostDecisions(c *gin.Context) {
	var newDecision models.PlayerDecision

	// Call BindJSON to bind the received JSON to
	// newAlbum.
	if err := c.BindJSON(&newDecision); err != nil {
		return
	}

	// Add the new album to the slice.
	playerDecisions = append(playerDecisions, newDecision)
	c.IndentedJSON(http.StatusCreated, newDecision)
}

// getAlbumByID locates the album whose ID value matches the id
// parameter sent by the client, then returns that album as a response.
func GetDecisionByID(c *gin.Context) {
	id := c.Param("id")

	// Loop through the list of albums, looking for
	// an album whose ID value matches the parameter.
	for _, d := range playerDecisions {
		if d.PlayerID == id {
			c.IndentedJSON(http.StatusOK, d)
			return
		}
	}
	c.IndentedJSON(http.StatusNotFound, gin.H{"message": "Decision not found"})
}
