
SELECT P.name, W.name, R.quantity, U.name FROM Product P
JOIN Recipe R ON R.productId = P.id
JOIN Warehouse_Item W ON W.id = R.warehouseItemId
JOIN Unit U ON U.id = W.unitId
ORDER BY P.name