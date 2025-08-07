package main

type Vector3 struct {
	X float32`json:"x"`
	Y float32`json:"y"`
	Z float32`json:"z"`
}

type PlayerData struct {
	PlayerName string 		`json:"playerName"`
	PlayerHealth int 		`json:"playerHealth"`
	Score int 				`json:"score"`
	PlayerPosition Vector3 	`json:"playerPosition"`
}
