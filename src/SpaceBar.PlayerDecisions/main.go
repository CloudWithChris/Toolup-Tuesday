// here is a change
package main

import (
	"github.com/cloudwithchris/toolup-tuesday/src/SpaceBar.PlayerDecisions/controllers"
	"github.com/gin-gonic/gin"
)

func main() {
	router := gin.Default()
	router.GET("/decisions", controllers.GetDecisions)
	router.GET("/decisions/:id", controllers.GetDecisionByID)
	router.POST("/decisions", controllers.PostDecisions)

	router.Run("localhost:8080")
}
