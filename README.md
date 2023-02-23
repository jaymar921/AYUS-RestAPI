# API DOCUMENTATION
# AYUS API DOCUMENTATION
### Prepared by Jayharron Mar Abejar
> Used for frontend and backend interaction. The front-end system was built on a different framework [React for Client/Mechanic]() and [ASP for Admin](), this system is build for backend operation.

&nbsp;

# ACCOUNT
### GET Account
```JavaScript
fetch("https://localhost:7172/api/Account", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"username": "---",  // Use Username & Password to retrieve account
		"password": "---", // Use Username & Password to retrieve account
		"uuid": "xxxxx", // Use UUID retrieve account  *optional*
		"option": "all" // Retrieves an array of all accounts  *optional*
	}
})
```

### POST account
```JavaScript
fetch("https://localhost:7172/api/Account", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"Content-Type": "application/json"
	},
	body: {
	    ...
	}
})
```

### PUT account [reset password]
```JavaScript
fetch("https://localhost:7172/api/Account/Password?uuid=****", {
	method: "PUT",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"new-password":"----"
	}
})
```

# Session
### Register a session
```JavaScript
fetch("https://localhost:7172/api/Sessions/RegisterSession", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
        "ClientUUID": "xxxxx",
        "MechanicUUID": "xxxxx",
		"SessionDetails": "xxxxx"
	}
})
```



### Get a session
```JavaScript
fetch("https://localhost:7172/api/Sessions/GetSession", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"ClientUUID": "xxxxx",     // if kabalo ka sa uid sa client, then only specify this
		"MechanicUUID": "xxxxx",   // if kabalo ka sa uid sa mechanic, then only specify this
		"SessionID": "xxxxx"       // if kabalo ka sa uid sa session, then only specify this
	} 
})
```


### Get an available mechanic
```JavaScript
fetch("https://localhost:7172/api/Sessions/AvailableMechanics", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX"
	} 
})
```

### End a session
```JavaScript
fetch("https://localhost:7172/api/Sessions/EndSession", {
	method: "PUT",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"ClientUUID": "xxxxx",     // if kabalo ka sa uid sa client, then only specify this
		"MechanicUUID": "xxxxx",   // if kabalo ka sa uid sa mechanic, then only specify this
		"SessionID": "xxxxx"       // if kabalo ka sa uid sa session, then only specify this
		"TransactionID": "xxxxx"   // THIS IS A MUST! dapat maka transact paka before maka end, [for testing, just put random string]
	} 
})
```

# Transaction
### POST 
 Adding a new transaction, this must be done first before ending a session
```JavaScript
fetch("https://localhost:7172/api/Transaction", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"Content-Type": "application/json"
	},
	body: {
		"ServiceName":"string",
		"ServicePrice":double,
		"Remark": "string"
	}
})
```


### GET TRANSACTION
```JavaScript
fetch("https://localhost:7172/api/Transaction", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"TransactionID": "xxxx"
	}
})
```

# SYSTEM

### RESET SYSTEM
This will reset the system's database, clears all data [USE THIS AT YOUR OWN RISK]
```JavaScript
fetch("https://localhost:7172/api/System/ResetDatabase", {
	method: "DELETE",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX"
	}
})
```

# Vehicles

### POST/PUT -ADDVEHICLE
```JavaScript
fetch("https://localhost:7172/api/Account/Vehicle", {
	method: "POST", // Change to 'PUT' if you are updating the data
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"Content-Type": "application/json"
	},
	body: {
		"uuid": "638eafb1-29e0-40dc-b234-e91d6bac96fe", // user [client/admin/mechanic] UUID
	  	"plateNumber": "GKR287", // this will be the primary key
	  	"brand": "Toyota",
	  	"model": "Revo",
	  	"type": "Manual Transmission",
	  	"color": "Navy Green"
	}
})
```

### DELETE - Remove Vehicle
```JavaScript
fetch("https://localhost:7172/api/Account/Vehicle", {
	method: "DELETE",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"PlateNumber": "XXXXXX" 
	}
})
```


### GET -Get Vehicles from USER
```JavaScript
fetch("https://localhost:7172/api/Account/Vehicle", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"uuid": "XXXXXX" 
	}
})
```

# Services
### POST/PUT - ADD SERVICES
```JavaScript
fetch("https://localhost:7172/api/System/Service", {
	method: "POST", // Change to 'PUT' if you are updating the data
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"Content-Type": "application/json"
	},
	body: {
	    "ServiceID": "string", // REQUIRED ON 'PUT' METHOD
	    "ServiceName": "string",
	    "ServiceDescription": "string"
	}
})
```

### GET SERVICES [ARRAY]
```JavaScript
fetch("https://localhost:7172/api/System/Service", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX"
	}
})
```


### DELETE SERVICE
```JavaScript
fetch("https://localhost:7172/api/System/Service", {
	method: "DELETE",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"ServiceID": "xxxxxxx"
	}
})
```

# WALLET
### Get user wallet
```JavaScript
fetch("https://localhost:7172/api/Wallet?uuid=XXXXX", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
	}
})
```

### Update user wallet
```JavaScript
fetch("https://localhost:7172/api/Wallet?uuid=XXXXX", {
	method: "PUT",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"newbalance": Number, // [REQUIRED]
		"pincode": "XXXXXX"   // [SPECIFY ONLY WHEN UPDATING]
	}
})
```

# MECHANIC
### Get the mechanic Shop
```JavaScript
fetch("https://localhost:7172/api/Mechanic/Shop", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED]
	}
})
```

### Update the mechanic Shop
```JavaScript
fetch("https://localhost:7172/api/Mechanic/Shop", {
	method: "PUT",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED]
		"Content-Type": "application/json"
	},
	body:{
		"ShopName": "HaroldShopee",
        "ShopDescription": "Pina ka poging Shop"
	}
})
```

## Service Offer [What services does the shop offer?]
### Add ServiceOffer to the Mechanic Shop
```JavaScript
fetch("https://localhost:7172/api/Mechanic/Shop", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED]
		"Content-Type": "application/json"
	},
	body:{
		"serviceID": "XXXXXX", // Make sure that the Service exists already, [do [GET]api/System/Service first]
		"price": Number,
		"serviceExpertise": "I work on a company before with battery jumpstart related, I have confident on this skill"
	}
})
```

### Get ServiceOffer from the Mechanic Shop [ARRAY]
```JavaScript
fetch("https://localhost:7172/api/Mechanic/Shop", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED]
	}
})
```


### Delete ServiceOffer from the Mechanic Shop
```JavaScript
fetch("https://localhost:7172/api/Mechanic/Shop", {
	method: "DELETE",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED]
		"ServiceOfferUUID": "XXXXXX" // [REQUIRED]
	}
})
