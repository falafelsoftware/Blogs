define([
	'require',
	'jquery',
	'can'
], function (require, $, can) {
	var Conference = can.Model({
		//TODO: USE FIXTURES PLUGIN TO SIMULATE AJAX
		//http://canjs.us/#plugins-can_fixture
		findAll: 'GET ' + require.toUrl('../../Services/conferences/all.json'),
		findOne: 'GET ' + require.toUrl('../../Services/conferences/{id}.json'),
		create:  'POST ' + require.toUrl('../../Services/conferences/all.json'),
		update:  'PUT ' + require.toUrl('../../Services/conferences/{id}.json'),
		destroy: 'DELETE ' + require.toUrl('../../Services/conferences/{id}.json')
	}, {
		imageUrl: function () {
			//TODO: ERROR WHEN USED IN EJS TEMPLATE: Uncaught TypeError: Object #<Object> has no method 'imageUrl'
			return require.toUrl('../../Content/images/logo.' + this.BaseName + '.png');
		},
		startDateFriendly: function () {
			//TODO: ERROR WHEN USED IN EJS TEMPLATE
			return moment(this.StartDate).format('MMM DD, YYYY');
		},
		endDateFriendly: function () {
			//TODO: ERROR WHEN USED IN EJS TEMPLATE
			return moment(this.StartDate).format('MMM DD, YYYY');
		}
	});

	return Conference;
});