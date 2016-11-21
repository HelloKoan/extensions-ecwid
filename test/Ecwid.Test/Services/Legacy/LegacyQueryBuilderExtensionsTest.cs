﻿// Licensed under the GPL License, Version 3.0. See LICENSE in the git repository root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Ecwid.Legacy;
using Xunit;

namespace Ecwid.Test.Services.Legacy
{
	[SuppressMessage("ReSharper", "ExceptionNotDocumented")]
	[SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
	public class LegacyQueryBuilderExtensionsTest
	{
		private readonly IEcwidLegacyClient _defaultLegacyClient = new EcwidLegacyClient();

		[Fact]
		public void ExtensionFail()
		{
			Assert.Throws<ArgumentException>(() => _defaultLegacyClient.Orders.Order(null));
			Assert.Throws<ArgumentException>(() => _defaultLegacyClient.Orders.FromOrder(null));
			Assert.Throws<ArgumentException>(() => _defaultLegacyClient.Orders.Order(-1));
			Assert.Throws<ArgumentException>(() => _defaultLegacyClient.Orders.FromOrder(-1));
			Assert.Throws<ArgumentException>(() => _defaultLegacyClient.Orders.Limit(-1));
			Assert.Throws<ArgumentException>(() => _defaultLegacyClient.Orders.Offset(-1));
		}

		[Fact]
		public void Date()
		{
			var result = _defaultLegacyClient.Orders.Date(Convert.ToDateTime("2000-01-01")).GetParam("date");
			var result2 = _defaultLegacyClient.Orders.FromDate(Convert.ToDateTime("2000-01-01")).GetParam("from_date");
			var result3 = _defaultLegacyClient.Orders.ToDate(Convert.ToDateTime("2000-01-01")).GetParam("to_date");
			var result4 =
				_defaultLegacyClient.Orders.FromUpdateDate(Convert.ToDateTime("2000-01-01")).GetParam("from_update_date");
			var result5 =
				_defaultLegacyClient.Orders.ToUpdateDate(Convert.ToDateTime("2000-01-01")).GetParam("to_update_date");

			Assert.Equal(result, "2000-01-01");
			Assert.Equal(result2, "2000-01-01");
			Assert.Equal(result3, "2000-01-01");
			Assert.Equal(result4, "2000-01-01");
			Assert.Equal(result5, "2000-01-01");
		}

		[Fact]
		public void Order()
		{
			var result = _defaultLegacyClient.Orders.Order(1).GetParam("order");
			var result2 = _defaultLegacyClient.Orders.FromOrder(1).GetParam("from_order");
			var result3 = _defaultLegacyClient.Orders.Order("1").GetParam("order");
			var result4 = _defaultLegacyClient.Orders.FromOrder("1").GetParam("from_order");

			Assert.Equal(result, 1);
			Assert.Equal(result2, 1);
			Assert.Equal(result3, "1");
			Assert.Equal(result4, "1");
		}

		[Fact]
		public void Customer()
		{
			var result = _defaultLegacyClient.Orders.CustomerId(1).GetParam("customer_id");
			var result2 = _defaultLegacyClient.Orders.CustomerId(null).GetParam("customer_id");
			var result3 = _defaultLegacyClient.Orders.CustomerEmail("test").GetParam("customer_email");
			var result4 = _defaultLegacyClient.Orders.CustomerEmail(null).GetParam("customer_email");
			var result5 = _defaultLegacyClient.Orders.CustomerEmail("").GetParam("customer_email");

			Assert.Equal(result, 1);
			Assert.Equal(result2, "null");
			Assert.Equal(result3, "test");
			Assert.Equal(result4, "");
			Assert.Equal(result5, "");
		}

		[Fact]
		public void Statuses()
		{
			var result = _defaultLegacyClient.Orders.Statuses("PAID, DECLINED", "NEW PROCESSING").GetParam("statuses");
			Assert.Equal(result, "PAID,DECLINED,NEW,PROCESSING");
		}

		[Theory]
		[InlineData("PAID, DECLIN", "NEW PROCESSING")]
		[InlineData("PAID, DECLINED", "NEW PROCESS")]
		[InlineData("", "")]
		public void StatusesFail(string paid, string full)
		{
			Assert.Throws<EcwidConfigException>(() => _defaultLegacyClient.Orders.Statuses(paid, full));
		}

		[Theory]
		[InlineData("DECLINED, CANCELLED")]
		[InlineData("DECLINED,CANCELLED")]
		[InlineData("DECLINED ,CANCELLED")]
		[InlineData("DECLINED CANCELLED")]
		public void PaymentStatuses(string statuses)
		{
			var result = _defaultLegacyClient.Orders.PaymentStatuses(statuses).GetParam("statuses");
			Assert.Equal(result, "DECLINED,CANCELLED");
		}

		[Theory]
		[InlineData("PAID, DECLINE")]
		[InlineData("DECLINED, CANCEL")]
		[InlineData("DECLINED,CANCEL")]
		[InlineData("DECLINED ,CANCEL")]
		[InlineData("DECLINED CANCELLED FAIL")]
		[InlineData("")]
		public void PaymentStatusesFail(string paid)
		{
			Assert.Throws<EcwidConfigException>(() => _defaultLegacyClient.Orders.PaymentStatuses(paid));
		}

		[Fact]
		public void AddTwicePaymentStatuses()
		{
			var result =
				_defaultLegacyClient.Orders.PaymentStatuses("PAID, DECLINED").PaymentStatuses("Cancelled").GetParam(
					"statuses");
			Assert.Equal(result, "PAID,DECLINED,CANCELLED");
		}

		[Fact]
		public void FulfillmentStatuses()
		{
			var result = _defaultLegacyClient.Orders.FulfillmentStatuses("NEW PROCESSING").GetParam("statuses");
			Assert.Equal(result, "NEW,PROCESSING");
		}

		[Fact]
		public void AddFulfillmentStatuses()
		{
			var result =
				_defaultLegacyClient.Orders.FulfillmentStatuses("NEW").FulfillmentStatuses("PROCESSING").GetParam(
					"statuses");
			Assert.Equal(result, "NEW,PROCESSING");
		}

		[Theory]
		[InlineData("PAID, DECLINE")]
		[InlineData("")]
		public void FulfillmentStatusesFail(string paid)
		{
			Assert.Throws<EcwidConfigException>(() => _defaultLegacyClient.Orders.FulfillmentStatuses(paid));
		}

		[Fact]
		public void AddBothStatuses()
		{
			var result =
				_defaultLegacyClient.Orders.PaymentStatuses("PAID, DECLINED")
					.FulfillmentStatuses("NEW PROCESSING")
					.GetParam("statuses");
			Assert.Equal(result, "PAID,DECLINED,NEW,PROCESSING");
		}

		[Fact]
		public void Limit()
		{
			var result = _defaultLegacyClient.Orders.Limit(120).GetParam("limit");
			var result2 = _defaultLegacyClient.Orders.Limit(201).GetParam("limit");

			Assert.Equal(result, 120);
			Assert.Equal(result2, 200);
		}
	}
}