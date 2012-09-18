define([
	'jquery',
	'can',
	'controllers/Conferences'
], function ($, can, Conferences) {

	var Router = can.Control({
		//HOME PAGE
		'route': function () {
			Conferences.list();
		},

		'conferences route': function () {
			Conferences.list();
		},

		'conferences/grid route': function () {
			Conferences.grid();
		},

		'conferences/:id route': function (data) {
			Conferences.detail(data);
		}
	});

	return new Router(document);
});