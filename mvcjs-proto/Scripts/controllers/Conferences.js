define([
	'jquery',
	'can',
	'kendoweb',
	'moment',
	'models/Conference'
], function ($, can, kendo, moment, Conference) {

	var Conferences = can.Control({
		//INITIALIZE
		init: function (element, options) {

		},

		//EVENTS
		'.list > li click': function (element, event) {
			//GET THE MODEL DATA
			var model = element.data('conference');

			//ONLY ALLOW AVAILABLE DEMO DATA TO BE CLICKED
			if (model.Id == 18) {
				//NAVIGATE TO DETAIL PAGE
				window.location.hash = '!conferences/' + model.Id;
			} else {
				alert('Only "FalafelCon" works for demo!"');
			}
		},

		//ACTIONS
		list: function (data) {
			//GET ALL ITEMS FROM STORAGE
			Conference.findAll({}, function(response) {
				//BUILD VIEW MODEL
				var models = {
					conferences: response.Conferences
				};

				//PASS RESULTS TO VIEW
				var view = can.view(require.toUrl('views/conferences/List.ejs'), models);

				//RENDER IN SPECIFIED CONTENT AREA
				$('#main').html(view);
			});

			//TODO: USE DEFERREDS ABOVE INSTEAD?
			//http://canjs.us/#can_view-deferreds
			/*
			can.view(require.toUrl('views/conferences/List.ejs'), {
				//PASS RESULTS TO VIEW
				//TODO: HOW TO SPECIFY ROOT NODE?
				conferences: Conference.findAll()
			}).then(function (content) {
				//RENDER IN SPECIFIED CONTENT AREA
				$('#main').html(content);
			});
			*/
		},

		detail: function (data) {
			//GET ITEM FROM STORAGE
			Conference.findOne({ id: data.id }, function(response) {
				//BUILD VIEW MODEL
				var models = {
					conference: response.ConferenceInfo,
					meta: response.Conference
				};

				//PASS RESULTS TO VIEW
				var view = can.view(require.toUrl('views/conferences/Detail.ejs'), models);

				//RENDER IN SPECIFIED CONTENT AREA
				$('#main').html(view);
			});
		},

		grid: function (data) {
			//GET ITEM FROM STORAGE
			Conference.findAll({}, function(response) {
				//BUILD VIEW MODEL
				var models = {
					conferences: response.Conferences
				};

				//PASS RESULTS TO VIEW
				var view = can.view(require.toUrl('views/conferences/Grid.ejs'));

				//RENDER IN SPECIFIED CONTENT AREA
				$('#main').html(view);

				//INITIALIZE KENDO GRID
				$("#main .grid").kendoGrid({
					dataSource: {
						data: models.conferences,
						pageSize: 10
					},
					sortable: true,
					pageable: true,
					columns: [
						{
							field: "Id",
							title: "Id",
							width: 50
						},
						{
							field: "Name",
							title: "Name"
						},
						{
							field: "LocationFriendly",
							title: "Location"
						},
						{
							field: "WebSiteURL",
							title: "WebSite"
						},
						{
							field: "StartDate",
							title: "Start Date",
							template: '#= moment(StartDate).format("MMM DD, YYYY") #'
						}
					]
				});
			});
		}
	});

	return new Conferences(document);
});