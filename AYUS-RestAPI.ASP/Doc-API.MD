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
		"username": "---",  // Use Username & Password to retrieve account [Must come with password]
		"password": "---", // Use Username & Password to retrieve account [Must come with username]
		"uuid": "xxxxx", // Use UUID retrieve account  *optional*
		"option": "all" // Retrieves an array of all accounts  *optional*
	}
})
```

### POST/PUT account
```JavaScript
fetch("https://localhost:7172/api/Account", {
	method: "POST", // Change to 'PUT' if you are updating the data
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"Content-Type": "application/json"
	},
	body: {
	    ...
	}
})
```

POST and PUT for Clients/Mechanic/Admin's data, for Mechanic, the "Shop" should not be null. In order to update, do [/api/Mechanic/Shop](#get-the-mechanic-shop)
```JavaScript
// POST/PUT body for Clients/Mechanics/Admins
{
    "personalInformation": {
        "uuid": String, // specify ONLY on 'PUT' method
        "Firstname": String,
        "Lastname": String,
        "Contact": String,
        "Birthdate": DateTime,
        "Address": String,
        "LicenseNumber": String,
        "Expiry": DateTime
    },
    "credential": {
        "Username": String,
        "Password": String, // will be hashed in the server
        "Email": String
    },
    "wallet": {
        "Balance": Number,
        "Pincode": String
    },
    "accountStatus": {
        "Shop": {   // SET SHOP = 'null' if not MECHANIC, this is fixed, to update, do [/api/Mechanic/Shop]()
            "ShopName": String,
            "ShopDescription": String
        },
        "Role": "CLIENT" // MECHANIC/ADMIN
    }
}
```

### Update Personal Information
```JavaScript
fetch("https://localhost:7172/api/Account", {
	method: "PUT",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
	},
	body: {
		"UUID": "XXXX", // [REQUIRED]
		"Firstname": String,
		"Lastname": String,
		"Contact": String,
		"Birthdate": "YYYY-MM-DDT00:00:00",
		"Address": String,
		"LicenseNumber": String,
		"Expiry": "YYYY-MM-DDT00:00:00"
	}
})


// OUTPUT [if success]
{
    "Status": 200,
    "Message": "Updated Personal Information for user 'user'",
    "UpdatedInformation": {
        "UUID": "uuid",
        "Firstname": "new Firstname",
        "Lastname": "new Lastname",
        "Contact": "new Contact",
        "Birthdate": "new Birthdate",
        "Address": "new Address",
        "LicenseNumber": "new License Number",
        "Expiry": "new Expiry"
    },
    "OldInformation": {
        "UUID": "uuid",
        "Firstname": "old Firstname",
        "Lastname": "old Lastname",
        "Contact": "old Contact",
        "Birthdate": "old Birthdate",
        "Address": "old Address",
        "LicenseNumber": "old License Number",
        "Expiry": "old Expiry"
    }
}
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

### Delete Account
```JavaScript
fetch("https://localhost:7172/api/Account", {
	method: "DELETE",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"uuid": "xxxxx", // Use UUID delete account
	}
})
```

### Get User Rating
```JavaScript
fetch("https://localhost:7172/api/Account/Rating", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"uuid": "xxxxx", // [Required]
	}
})

// Returns 200 if found, else 404
{
	Status = 200, 
	Message = "Rating found for user '{Username}'", 
	Rating = 0 // [double]
}
```

### Update User Rating
```JavaScript
fetch("https://localhost:7172/api/Account/Rating", {
	method: "PUT",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"uuid": "xxxxx", // [Required]
	},
	body:{
		"Rating": 0 // [double]
	}
})

// Returns 200 if success, else 404
{
	Status = 200, 
	Message = "Updated Rating for user '{Username}'", 
	UpdatedRating = 0, // [double]
	PreviousRating = 0, // [double]
}
```

# Session
### Register a session
> Registering a session will remove a mechanic from [Available Sessions](#get-an-available-mechanic). It will also create a MapLocation Service. You can [Update](#update-a-maplocation) or [Retrieve](#get-a-maplocation) a map service data. A [Flag](#update-a-flag) is also created after registering a session.

> The MapLocation data is auto filled with [TemporaryMapLocation](#get-maplocation) if the values exists but it will not update overtime, you must call an [[Update]MapLocation](#update-a-maplocation).
```JavaScript
fetch("https://localhost:7172/api/Sessions/RegisterSession", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"ClientUUID": "xxxxx", // [REQUIRED]
		"MechanicUUID": "xxxxx", // [REQUIRED]
		"SessionDetails": "xxxxx", // [REQUIRED]
		"Flag": "Custom Flag" // *optional*
	}
})
```
### Update a Flag
> You need to know the SessionID in order to grab the flag, not that this is not persistent. If the server reloads or crashes, the flag data is gone.
```JavaScript
fetch("https://localhost:7172/api/Sessions/Flag", {
	method: "PUT",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"SessionID":"XXXXXXX", // [REQUIRED]
		"Flag": "Custom Flag" // [REQUIRED]
	}
})
```
### Get an available mechanic
> Note: This will ony show mechanics that are not in session, make sure that if a mechanic is in session, you must call EndSession to make it available.
```JavaScript
fetch("https://localhost:7172/api/Sessions/AvailableMechanics", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX"
	} 
})
```

### Get a session
> If you want to know more about the session, you can retrieve the data using this API link
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

### Get a MapLocation
> For each session created, a new MapLocation service is also created with it. You must know the SessionID to retrieve it's data.
```JavaScript
fetch("https://localhost:7172/api/Sessions/MapLocation", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"SessionID": "XXXXX" // [REQUIRED]
	} 
})
```

### Update a MapLocation
> When updating a MapLocation, there will be two devices involved and you cannot get the location of both at once. If you can track the client's location, then only update the client's location, otherwise do the same to the mechanic.
```JavaScript
fetch("https://localhost:7172/api/Sessions/MapLocation", {
	method: "PUT",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"SessionID": "XXXXX", // [REQUIRED]
		"ClientLocLat": Number, // Must come with Lon *Optional if only updating mechanic's location*
		"ClientLocLon": Number, // Must come with Lat *Optional if only updating mechanic's location*
		"MechanicLocLat": Number, // Must come with Lon *Optional if only updating client's location*
		"MechanicLocLon": Number, // Must come with Lat *Optional if only updating client's location*
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
		"TransactionID": "xxxxx",   // THIS IS A MUST! dapat maka transact paka before maka end, [for testing, just put random string]
		"Flag": "Custom Flag" // *optional*
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

// OUTPUT IF [SUCCESS]
{
    "Status": 200,
    "Message": "Wallet of 'user' was found",
    "WalletData": {
        "Balance": Number,
        "Pincode": "string"
    }
}
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
		"option":"all", // [optional] once called, mechanicUUID is nothing, [returns an array]
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
> Note: You have to know the service ID first before adding a service offer. See [Get Service](#get-services-array)
```JavaScript
fetch("https://localhost:7172/api/Mechanic/ServiceOffer", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED] <- personalInfo.UUID not the Shop.ShopID
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
fetch("https://localhost:7172/api/Mechanic/ServiceOffer", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED]
	}
})
```


### Delete ServiceOffer from the Mechanic Shop
```JavaScript
fetch("https://localhost:7172/api/Mechanic/ServiceOffer", {
	method: "DELETE",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED]
		"ServiceOfferUUID": "XXXXXX" // [REQUIRED]
	}
})
```

# Service Request
### Get a list of service Request from a mechanic UUID or a specific service request from ServiceRequestUuid
```JavaScript
fetch("https://localhost:7172/api/ServiceRequest", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"MechanicUUID": "XXXXX", // [REQUIRED]
		"ServiceRequestUUID": "XXXXXX" // *optional*
	}
})
```

### Register a service Request
```JavaScript
fetch("https://localhost:7172/api/ServiceRequest", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"Content-Type": "application/json"
	},
	body: {
		"requestor": String, // CLIENT UUID
		"recepient": String, // MECHANIC UUID
		"contact": String,
		"location": String,
		"vehicle": String,
		"service": String,
		"description": String,
		"picture": null // this will be an object in the backend
	}
})
```

### Delete a service request
> Before deleting, you must get the ServiceRequest data and store it in the SessionDetails [Only if the mechanic accepts the offer]
```JavaScript
fetch("https://localhost:7172/api/ServiceRequest", {
	method: "DELETE",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"ServiceRequestUUID": "XXXXXX" // [REQUIRED]
	}
})
```

# Billing [Mechanic]
> After a successful session, a billing should follow from where it deducts the balance from the mechanic's wallet. The mechanic cannot be available to serve if the [wallet's](#wallet) balance is less than the service rate.
### Saving a billing information
```JavaScript
fetch("https://localhost:7172/api/Mechanic/Billing", {
	method: "POST",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"ShopID": "XXXXXX", // [REQUIRED]
		"Content-Type": "application/json"
	},
	body: {
		{
			"shopID": "string",
			"serviceFee": 0,
			"serviceRemark": "string"
		}
	}
})
```
### Retrieving a billing information
```JavaScript
fetch("https://localhost:7172/api/Mechanic/Billing", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"ShopID": "XXXXXX" // [REQUIRED]
	}
})

// OUTPUT IF [SUCCESS]
{
	"Status": 200,
	"Message": "Billing information found from shop provided",
	"BillingData": [
		{
			"BillingID": "string",
			"ShopID": "string",
			"BillingDate": "string",
			"ServiceFee": Number,
			"ServiceRemark": "string"
		}
	]
}
```
### Deleting a billing information
```JavaScript
fetch("https://localhost:7172/api/Mechanic/Billing", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"BillingID": "XXXXXX" // [REQUIRED]
	}
})
```
# Temporary Data Route
### Get-MapLocation
> Note that this is not stored in the database, for temporary call only. To save the location, use [Session](#session) instead
```JavaScript
fetch("https://localhost:7172/api/TemporaryRoute/MapLocation", {
	method: "GET",
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
		"UUID": "XXXXXX" // [REQUIRED]
	}
})

// Returns [If data found]
{
    "Status": 200,
    "Message": "Found data from the UUID specified",
    "Data": {
        "UUID": "XXXXXXX",
        "Latitude": Number,
        "Longitude": Number,
        "AdditionData": "string"
    }
}
```

###  Post/Put-MapLocation
> Note that this is not stored in the database, for temporary call only. To save the location, use [Session](#session) instead
```JavaScript
fetch("https://localhost:7172/api/TemporaryRoute/MapLocation", {
	method: "POST", // use 'PUT' method when updating
	headers:{
		"AYUS-API-KEY":"XXXXXXXX",
	}, 
	body: {
		"uuid": "string", // any UUID as long as you can access it.
		"latitude": 0,
		"longitude": 0,
		"additionData": "string"	
	}
})
```