package main

import (
	"encoding/json"
	"net/http"
	"os"

	"github.com/gin-gonic/gin"
)

func savePlayerData(c *gin.Context) {
	var player PlayerData
	// bind the request body to a player struct
	if err := c.ShouldBindJSON(&player); err != nil {
		c.JSON(http.StatusBadRequest, err.Error())
		return
	}
	
	bytes, err := json.MarshalIndent(player, "", "	")
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
		return
	}

	// write to file
	if err := os.WriteFile("data.json", bytes, 0644); err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
		return
	}

	c.IndentedJSON(http.StatusOK, player)
}

func getPlayerData(c *gin.Context) {
	bytes, err := os.ReadFile("data.json")
	if err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
		return
	}

	var player PlayerData
	if err := json.Unmarshal(bytes, &player); err != nil {
		c.JSON(http.StatusInternalServerError, err.Error())
		return
	}

	c.IndentedJSON(http.StatusOK, player)
}

func main() {
	router := gin.Default()
	router.GET("/data", getPlayerData)
	router.PUT("/data", savePlayerData)
	
	router.Run("localhost:8000")
}
