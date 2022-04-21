package models

// album represents data about a record album.
type PlayerDecision struct {
	DecisionID string `json:"decisionId"`
	PlayerID   string `json:"playerId"`
	PlayerName string `json:"playerName"`

	Decision     string `json:"decision"`
	WorldEventID string `json:"worldEventId"`
	DecisionDate string `json:"decisionDate"`
}
