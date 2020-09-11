# TopUtility

This utility provides API to see which processes runs on server side.

Example deployed here: https://toputility.azurewebsites.net

## Protocol transport

This project uses websocket technology.

## Api references

Address to connect: wss://toputility.azurewebsites.net/ws

Server can receive 3 actions: Show, Subscribe, Unsubscribe in json format.

Example:
```json
{
	"action": "Show"
}
```

Response example:
```json
[
	{
		"id": 0,
		"name": "Idle",
		"memory": 8
	},
	{
		"id": 762,
		"name": "DotNet",
		"memory": 666666
	}
]
```

Note: Memory in Kb

Your client can request one-time information about running processes using "show" command.

Your client can subscribe for broadcast using "subscribe" command. In this case server will update you with processes information automatically.

Your client can unsubscribe from broadcast using "unsubscribe" command. In this case server stops send you messages.

