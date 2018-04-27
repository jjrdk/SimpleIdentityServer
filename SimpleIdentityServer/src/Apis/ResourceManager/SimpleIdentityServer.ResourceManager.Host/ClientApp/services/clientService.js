import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';

module.exports = {
	/**
	* Search the clients.
	*/
	search: function(request, type) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.apiUrl + '/clients/'+type+'/.search',
                method: "POST",
                data: data,
                contentType: 'application/json',
                headers: {
                	"Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
		});
	},
	/**
	* Get the client.
	*/
	get: function(id) {
		return new Promise(function(resolve, reject) {
            $.get(Constants.apiUrl + '/clients/'+id).then(function(data) {
				resolve(data);
			}).fail(function(e) {
				reject(e);
			})
		});
	}
};