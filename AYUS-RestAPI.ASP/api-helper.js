// This is will be my way of fetching data, it's all up to you :D

const api_key = 'API_SECRET-42e016b219421dc83d180bdee27f81dd'; //LETS SAY THIS API KEY IS STORED SOMEWHERE

// API CLASS THAT IS CAPABLE OF POSTING AND GETTING DATA
class API {
    // construct the class once, just pass the url
    constructor(url) {
        this.url = url;
    }

    // post method
    async post(route, header, body = '') {
        return fetch(`${this.url}/${route}`, {
            method: 'POST',
            headers: { "AYUS-API-KEY": api_key, ...header },
            body: JSON.stringify(body)
        }).then(r => r.json());
    }

    // get method
    async get(route, header) {
        return fetch(`${this.url}/${route}`, {
            method: 'GET',
            headers: { "AYUS-API-KEY": api_key, ...header }
        }).then(r => r.json())
    }

    // put method
    async put(route, header, body) {
        return fetch(`${this.url}/${route}`, {
            method: 'PUT',
            headers: { "AYUS-API-KEY": api_key, ...header },
            body: JSON.stringify(body)
        }).then(r => r.json())
    }
}



// TEST FUNCTION [GET], it should be inside an asynchronous function
async function test() {

    const api = new API('http://192.168.1.50:5206');

    // call the api and get the account
    api.get('api/Account', { "option": "all" }).then(data => console.log(data));

    // call the api and update something
    api.put('api/Session/Flag', { 'sessionID': 'someID' }, { 'Flag': 'My Flag' }).then(data => console.log(data));
}

// CALL THE FUNCTION
test();