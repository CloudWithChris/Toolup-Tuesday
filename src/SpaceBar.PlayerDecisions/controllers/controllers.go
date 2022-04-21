package controllers

import (
	"context"
	"encoding/json"
	"net/http"
	"time"

	"github.com/cloudwithchris/toolup-tuesday/src/SpaceBar.PlayerDecisions/models"
	dapr "github.com/dapr/go-sdk/client"
	"github.com/gin-gonic/gin"
)

var (
	ctx   = context.Background()
	store = "playerdecisions"
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
	// (0) Create the Dapr Client.
	// Return the error if there is one.
	client, err := dapr.NewClient()
	if err != nil {
		c.Error(err)
		return
	}

	var newDecision models.PlayerDecision
	if err := c.BindJSON(&newDecision); err != nil {
		c.Error(err)
		return
	}

	jsonData, err := json.Marshal(newDecision)
	if err != nil {
		c.Error(err)
		return
	}

	// (1) Create the new Player Decision.
	if err := client.SaveState(ctx, store, newDecision.DecisionID, jsonData, nil); err != nil {
		c.Error(err)
		return
	}

	c.IndentedJSON(http.StatusCreated, newDecision)

	/*
		var newDecision models.PlayerDecision

		// Call BindJSON to bind the received JSON to
		// newAlbum.
		if err := c.BindJSON(&newDecision); err != nil {
			return
		}

		// Add the new album to the slice.
		playerDecisions = append(playerDecisions, newDecision)
		c.IndentedJSON(http.StatusCreated, newDecision)*/
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
