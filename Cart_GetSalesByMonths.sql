USE [C32_LashGirl]
GO
/****** Object:  StoredProcedure [dbo].[Cart_GetSalesByMonths]    Script Date: 6/8/2017 3:11:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[Cart_GetSalesByMonths]
			@ProductId int
			,@Year int

AS
/*  TEST  CODE
declare @ProductId int = 16
		,@Year int = 2017

execute dbo.Cart_GetSalesByMonths
		@ProductId
		,@Year
*/

BEGIN

		SELECT
		  ProductId,
		  NumberOfSales,
		  Revenue,
		  [Month],
		  [Year],
		  Title,
		  ProductType
		FROM (SELECT
		  ProductId,
		  SUM(Quantity) AS NumberOfSales,
		  SUM(Cost) AS Revenue,
		  MONTH(CreatedDate) AS [Month],
		  YEAR(CreatedDate) AS [Year]
		FROM dbo.Cart
			WHERE ProductId = @ProductId
				AND YEAR(CreatedDate) = @Year
			GROUP BY ProductId,
				 MONTH(CreatedDate),
				 YEAR(CreatedDate)) AS sub
		INNER JOIN dbo.Product AS p
		  ON sub.ProductId = p.Id

END