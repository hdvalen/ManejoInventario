-- Script de inserción de datos para la base de datos de gestión de inventario
USE gestionInventario;

-- Insert para la tabla pais
INSERT INTO pais (nombre) VALUES 
('Colombia'),
('México'),
('Argentina'),
('España'),
('Chile'),
('Perú'),
('Ecuador'),
('Brasil'),
('Venezuela'),
('Estados Unidos'),
('Canadá'),
('Francia'),
('Italia'),
('Alemania'),
('Reino Unido');

-- Insert para la tabla region
INSERT INTO region (nombre, pais_id) VALUES 
('Antioquia', 1),
('Cundinamarca', 1),
('Valle del Cauca', 1),
('Ciudad de México', 2),
('Jalisco', 2),
('Buenos Aires', 3),
('Córdoba', 3),
('Madrid', 4),
('Cataluña', 4),
('Región Metropolitana', 5),
('Lima', 6),
('Pichincha', 7),
('São Paulo', 8),
('Caracas', 9),
('California', 10);

-- Insert para la tabla ciudad
INSERT INTO ciudad (nombre, region_id) VALUES 
('Medellín', 1),
('Bogotá', 2),
('Cali', 3),
('Ciudad de México', 4),
('Guadalajara', 5),
('Buenos Aires', 6),
('Córdoba', 7),
('Madrid', 8),
('Barcelona', 9),
('Santiago', 10),
('Lima', 11),
('Quito', 12),
('São Paulo', 13),
('Caracas', 14),
('Los Ángeles', 15);

-- Insert para la tabla empresa
INSERT INTO empresa (id, nombre, ciudad_id, fecha_registro) VALUES 
('E001', 'Distribuciones El Éxito', 1, '2018-05-12'),
('E002', 'Almacenes Metro', 2, '2017-10-25'),
('E003', 'Comercial Valle Grande', 3, '2019-03-18'),
('E004', 'Grupo Azteca', 4, '2016-08-30'),
('E005', 'Jalisco Distribuciones', 5, '2020-01-15'),
('E006', 'Pampa Comercial', 6, '2018-11-07'),
('E007', 'Córdoba Suministros', 7, '2019-07-22'),
('E008', 'Madrid Mayoristas', 8, '2017-04-13'),
('E009', 'Comercial Catalana', 9, '2020-02-28'),
('E010', 'Andina Distribuciones', 10, '2018-09-16'),
('E011', 'Perú Comercial', 11, '2019-05-05'),
('E012', 'Distribuidora Ecuatoriana', 12, '2017-12-10'),
('E013', 'Brasil Materiales', 13, '2020-03-20'),
('E014', 'Caracas Suministros', 14, '2018-06-30'),
('E015', 'California Trade', 15, '2019-11-25');

-- Insert para la tabla facturacion
INSERT INTO facturacion (fechaResolucion, numInicio, numFinal, factura_actual) VALUES 
('2023-01-01', 1000, 5000, 1050),
('2023-01-01', 5001, 10000, 5230),
('2023-03-15', 10001, 15000, 10125),
('2023-06-01', 15001, 20000, 15089),
('2023-08-15', 20001, 25000, 20346),
('2023-10-01', 25001, 30000, 25078),
('2024-01-01', 30001, 35000, 30421),
('2024-03-15', 35001, 40000, 35167),
('2024-06-01', 40001, 45000, 40098),
('2024-08-15', 45001, 50000, 45234),
('2024-10-01', 50001, 55000, 50045),
('2025-01-01', 55001, 60000, 55123),
('2025-03-15', 60001, 65000, 60078),
('2025-06-01', 65001, 70000, 65012),
('2025-08-15', 70001, 75000, 70003);

-- Insert para la tabla plan
INSERT INTO plan (nombre, fecha_inicio, fecha_fin, descuento) VALUES 
('Plan Básico', '2023-01-01', '2023-12-31', 5.00),
('Plan Premium', '2023-01-01', '2023-12-31', 10.00),
('Plan Empresarial', '2023-01-01', '2023-12-31', 15.00),
('Plan Verano', '2023-06-01', '2023-08-31', 7.50),
('Plan Navideño', '2023-11-15', '2024-01-15', 12.00),
('Plan Aniversario', '2023-05-01', '2023-05-31', 20.00),
('Plan Fidelidad', '2023-01-01', '2023-12-31', 8.00),
('Plan Especial', '2023-09-01', '2023-09-30', 15.00),
('Plan Nuevo Cliente', '2023-01-01', '2023-12-31', 10.00),
('Plan Black Friday', '2023-11-20', '2023-11-30', 25.00),
('Plan Semana Santa', '2023-04-01', '2023-04-15', 10.00),
('Plan Ejecutivo', '2023-01-01', '2023-12-31', 12.00),
('Plan Estudiante', '2023-01-01', '2023-12-31', 8.00),
('Plan Familia', '2023-01-01', '2023-12-31', 15.00),
('Plan Corporativo', '2023-01-01', '2023-12-31', 18.00);

-- Insert para la tabla producto
INSERT INTO producto (id, nombre, stock, stockMin, stockMax, fecha_creacion, fecha_actualizacion, codigo_barra) VALUES 
('P001', 'Laptop HP Pavilion 15', 50, 10, 100, '2022-01-15', '2023-05-10', '7890123456789'),
('P002', 'Monitor LG 24"', 35, 5, 60, '2022-02-20', '2023-04-05', '8901234567890'),
('P003', 'Teclado Mecánico Logitech', 45, 8, 80, '2022-03-10', '2023-03-15', '9012345678901'),
('P004', 'Mouse Inalámbrico Microsoft', 60, 12, 120, '2022-01-05', '2023-02-20', '0123456789012'),
('P005', 'Impresora HP LaserJet', 25, 5, 40, '2022-04-18', '2023-06-10', '1234567890123'),
('P006', 'Disco Duro Externo 1TB', 30, 6, 50, '2022-05-22', '2023-07-05', '2345678901234'),
('P007', 'Memoria USB 64GB', 100, 20, 200, '2022-01-30', '2023-05-25', '3456789012345'),
('P008', 'Tarjeta Gráfica NVIDIA GTX 1660', 15, 3, 30, '2022-06-15', '2023-04-30', '4567890123456'),
('P009', 'Procesador Intel i7', 20, 4, 35, '2022-07-10', '2023-03-20', '5678901234567'),
('P010', 'Audífonos Bluetooth Sony', 40, 8, 70, '2022-08-05', '2023-02-15', '6789012345678'),
('P011', 'Cámara Web Logitech HD', 35, 7, 60, '2022-09-20', '2023-01-10', '7890123456780'),
('P012', 'Router WiFi TP-Link', 25, 5, 45, '2022-10-15', '2023-06-20', '8901234567801'),
('P013', 'Altavoces Bluetooth JBL', 30, 6, 50, '2022-11-10', '2023-07-15', '9012345678012'),
('P014', 'Batería Externa 10000mAh', 50, 10, 90, '2022-12-05', '2023-05-30', '0123456789013'),
('P015', 'Adaptador HDMI', 70, 15, 120, '2022-12-20', '2023-04-25', '1234567890124');

-- Insert para la tabla plan_producto
INSERT INTO plan_producto (plan_id, producto_id) VALUES 
(1, 'P001'),
(1, 'P002'),
(1, 'P003'),
(2, 'P004'),
(2, 'P005'),
(2, 'P006'),
(3, 'P007'),
(3, 'P008'),
(3, 'P009'),
(4, 'P010'),
(4, 'P011'),
(5, 'P012'),
(5, 'P013'),
(6, 'P014'),
(6, 'P015');

-- Insert para la tabla tipoMovCaja
INSERT INTO tipoMovCaja (nombre, tipoMovimiento) VALUES 
('Venta', 'Entrada'),
('Compra', 'Salida'),
('Devolución Cliente', 'Salida'),
('Devolución Proveedor', 'Entrada'),
('Pago Nómina', 'Salida'),
('Pago Servicios', 'Salida'),
('Inversión', 'Salida'),
('Préstamo Bancario', 'Entrada'),
('Pago Impuestos', 'Salida'),
('Venta Activos', 'Entrada'),
('Comisiones', 'Entrada'),
('Gastos Administrativos', 'Salida'),
('Alquiler', 'Salida'),
('Mantenimiento', 'Salida'),
('Seguro', 'Salida');

-- Insert para la tabla tipo_documento
INSERT INTO tipo_documento (descripcion) VALUES 
('Cédula de Ciudadanía'),
('Tarjeta de Identidad'),
('Pasaporte'),
('Cédula de Extranjería'),
('DNI'),
('Registro Civil'),
('NIF/NIE'),
('SSN'),
('RFC'),
('CUIT'),
('RUT'),
('RUC'),
('CPF'),
('ID Nacional'),
('Licencia de Conducir');

-- Insert para la tabla tipo_tercero
INSERT INTO tipo_tercero (descripcion) VALUES 
('Cliente'),
('Proveedor'),
('Empleado'),
('Cliente/Proveedor'),
('Cliente/Empleado'),
('Proveedor/Empleado'),
('Administrador'),
('Inversionista'),
('Contratista'),
('Distribuidor'),
('Socio Comercial'),
('Consultor'),
('Transportista'),
('Representante Legal'),
('Agente Comercial');

-- Insert para la tabla tercero
INSERT INTO tercero (id, nombre, apellidos, email, tipo_documento_id, tipo_tercero_id, ciudad_id) VALUES 
('T001', 'Juan Carlos', 'Gómez Ramírez', 'jcgomez@email.com', 1, 1, 1),
('T002', 'María Fernanda', 'López Hernández', 'mflopez@email.com', 1, 2, 2),
('T003', 'Pedro José', 'Martínez Silva', 'pjmartinez@email.com', 1, 3, 3),
('T004', 'Ana Lucía', 'García Rodríguez', 'algarcia@email.com', 4, 1, 4),
('T005', 'Carlos Eduardo', 'Pérez Torres', 'ceperez@email.com', 5, 2, 5),
('T006', 'Laura Victoria', 'Díaz Morales', 'lvdiaz@email.com', 6, 3, 6),
('T007', 'Roberto Andrés', 'Sánchez Cruz', 'rasanchez@email.com', 7, 4, 7),
('T008', 'Sofía Alejandra', 'Fernández Castro', 'safernandez@email.com', 8, 5, 8),
('T009', 'Miguel Ángel', 'Torres Vargas', 'matorres@email.com', 9, 6, 9),
('T010', 'Valentina', 'Rojas Mendoza', 'vrojas@email.com', 10, 7, 10),
('T011', 'Daniel Esteban', 'Ortiz Navarro', 'deortiz@email.com', 11, 8, 11),
('T012', 'Camila Andrea', 'Castro Gutiérrez', 'cacastro@email.com', 12, 9, 12),
('T013', 'Jorge Alberto', 'Vargas Cardona', 'javargas@email.com', 13, 10, 13),
('T014', 'Patricia Elena', 'Moreno Jiménez', 'pemoreno@email.com', 14, 11, 14),
('T015', 'Andrés Felipe', 'Reyes Ospina', 'afreyes@email.com', 15, 12, 15);

-- Insert para la tabla cliente
INSERT INTO cliente (tercero_id, fecha_nacimiento, fecha_compra) VALUES 
('T001', '1985-05-10', '2022-01-15'),
('T004', '1990-03-22', '2022-02-20'),
('T007', '1978-11-30', '2022-03-10'),
('T008', '1992-07-05', '2022-01-05'),
('T010', '1988-09-18', '2022-04-18'),
('T011', '1995-01-25', '2022-05-22'),
('T013', '1980-06-12', '2022-01-30'),
('T014', '1993-04-08', '2022-06-15'),
('T015', '1987-12-15', '2022-07-10'),
('T002', '1982-08-28', '2022-08-05'),
('T005', '1991-02-14', '2022-09-20'),
('T006', '1975-10-03', '2022-10-15'),
('T009', '1998-03-27', '2022-11-10'),
('T003', '1983-11-19', '2022-12-05'),
('T012', '1989-07-22', '2022-12-20');

-- Insert para la tabla proveedor
INSERT INTO proveedor (tercero_id, descuento, dia_pago) VALUES 
('T002', 5.5, 15),
('T005', 8.0, 30),
('T007', 10.0, 10),
('T009', 7.5, 20),
('T012', 12.0, 5),
('T003', 6.0, 25),
('T006', 9.0, 15),
('T010', 11.0, 30),
('T013', 7.0, 10),
('T014', 8.5, 20),
('T015', 10.5, 5),
('T001', 6.5, 25),
('T004', 9.5, 15),
('T008', 12.5, 30),
('T011', 7.5, 10);

-- Insert para la tabla eps
INSERT INTO eps (nombre) VALUES 
('Sura EPS'),
('Compensar EPS'),
('Sanitas EPS'),
('Nueva EPS'),
('Salud Total'),
('Famisanar'),
('Coomeva EPS'),
('Medimás'),
('Mutual Ser'),
('Aliansalud'),
('Comfenalco Valle'),
('Coosalud'),
('Servicio Occidental de Salud'),
('Capital Salud'),
('Cruz Blanca');

-- Insert para la tabla arl
INSERT INTO arl (nombre) VALUES 
('Sura ARL'),
('Positiva'),
('Colmena Seguros'),
('AXA Colpatria'),
('Seguros Bolívar'),
('Liberty Seguros'),
('Mapfre'),
('La Equidad Seguros'),
('Alfa Seguros'),
('Mundial Seguros'),
('Seguros del Estado'),
('Allianz'),
('Previsora Seguros'),
('HDI Seguros'),
('Aseguradora Solidaria');

-- Insert para la tabla tercero_telefono
INSERT INTO tercero_telefono (numero, tercero_id, tipo_telefono) VALUES 
('3101234567', 'T001', 'Movil'),
('6017654321', 'T001', 'Fijo'),
('3207654321', 'T002', 'Movil'),
('6018765432', 'T002', 'Fijo'),
('3153456789', 'T003', 'Movil'),
('6019876543', 'T003', 'Fijo'),
('3129876543', 'T004', 'Movil'),
('6010987654', 'T004', 'Fijo'),
('3175678901', 'T005', 'Movil'),
('3208765432', 'T006', 'Movil'),
('3134567890', 'T007', 'Movil'),
('3146789012', 'T008', 'Movil'),
('3157890123', 'T009', 'Movil'),
('3168901234', 'T010', 'Movil'),
('3189012345', 'T011', 'Movil');

-- Insert para la tabla empleado
INSERT INTO empleado (tercero_id, fecha_ingreso, salario_base, eps_id, arl_id) VALUES 
('T003', '2020-01-15', 2500000, 1, 1),
('T006', '2020-03-10', 2800000, 2, 2),
('T008', '2020-02-20', 3000000, 3, 3),
('T009', '2020-05-05', 2700000, 4, 4),
('T010', '2020-04-18', 3200000, 5, 5),
('T001', '2020-06-30', 2900000, 6, 6),
('T002', '2021-01-10', 3100000, 7, 7),
('T004', '2021-02-15', 2600000, 8, 8),
('T005', '2021-03-20', 3300000, 9, 9),
('T007', '2021-04-25', 2750000, 10, 10),
('T011', '2021-05-30', 3050000, 11, 11),
('T012', '2021-06-15', 2850000, 12, 12),
('T013', '2022-01-20', 3150000, 13, 13),
('T014', '2022-02-25', 2950000, 14, 14),
('T015', '2022-03-30', 3250000, 15, 15);

-- Insert para la tabla movimientoCaja
INSERT INTO movimientoCaja (fecha, tipoMovimiento_id, valor, concepto, tercero_id) VALUES 
('2023-01-05', 1, 1500000.00, 'Venta de equipos informáticos', 'T001'),
('2023-01-10', 2, 800000.00, 'Compra de insumos', 'T002'),
('2023-01-15', 3, 200000.00, 'Devolución cliente por equipo defectuoso', 'T004'),
('2023-01-20', 4, 150000.00, 'Devolución a proveedor por productos en mal estado', 'T005'),
('2023-01-25', 5, 2500000.00, 'Pago nómina empleados enero', 'T003'),
('2023-02-05', 6, 350000.00, 'Pago servicios públicos', 'T006'),
('2023-02-10', 7, 5000000.00, 'Inversión en nueva sucursal', 'T007'),
('2023-02-15', 8, 10000000.00, 'Préstamo bancario para ampliación', 'T008'),
('2023-02-20', 9, 1200000.00, 'Pago impuestos primer bimestre', 'T009'),
('2023-02-25', 10, 3000000.00, 'Venta de mobiliario antiguo', 'T010'),
('2023-03-05', 11, 500000.00, 'Comisiones por ventas febrero', 'T011'),
('2023-03-10', 12, 300000.00, 'Gastos administrativos', 'T012'),
('2023-03-15', 13, 1500000.00, 'Pago alquiler local comercial', 'T013'),
('2023-03-20', 14, 450000.00, 'Mantenimiento equipos', 'T014'),
('2023-03-25', 15, 800000.00, 'Pago póliza de seguro anual', 'T015');

-- Insert para la tabla compra
INSERT INTO compra (terceroProveedor_id, fecha, terceroEmpleado_id, DocCompra) VALUES 
('T002', '2023-01-05', 'T003', 'FC-2023-001'),
('T005', '2023-01-10', 'T006', 'FC-2023-002'),
('T007', '2023-01-15', 'T008', 'FC-2023-003'),
('T009', '2023-01-20', 'T010', 'FC-2023-004'),
('T012', '2023-01-25', 'T001', 'FC-2023-005'),
('T003', '2023-02-05', 'T002', 'FC-2023-006'),
('T006', '2023-02-10', 'T004', 'FC-2023-007'),
('T010', '2023-02-15', 'T005', 'FC-2023-008'),
('T013', '2023-02-20', 'T007', 'FC-2023-009'),
('T014', '2023-02-25', 'T009', 'FC-2023-010'),
('T015', '2023-03-05', 'T011', 'FC-2023-011'),
('T001', '2023-03-10', 'T012', 'FC-2023-012'),
('T004', '2023-03-15', 'T013', 'FC-2023-013'),
('T008', '2023-03-20', 'T014', 'FC-2023-014'),
('T011', '2023-03-25', 'T015', 'FC-2023-015');

-- Insert para la tabla detalle_compra
INSERT INTO detalle_compra (fecha, producto_id, cantidad, valor, compra_id) VALUES 
('2023-01-05', 'P001', 5, 2500000.00, 1),
('2023-01-10', 'P002', 10, 1800000.00, 2),
('2023-01-15', 'P003', 15, 750000.00, 3),
('2023-01-20', 'P004', 20, 400000.00, 4),
('2023-01-25', 'P005', 3, 1500000.00, 5),
('2023-02-05', 'P006', 8, 960000.00, 6),
('2023-02-10', 'P007', 50, 500000.00, 7),
('2023-02-15', 'P008', 5, 2000000.00, 8),
('2023-02-20', 'P009', 7, 2800000.00, 9),
('2023-02-25', 'P010', 12, 840000.00, 10),
('2023-03-05', 'P011', 10, 700000.00, 11),
('2023-03-10', 'P012', 8, 640000.00, 12),
('2023-03-15', 'P013', 10, 800000.00, 13),
('2023-03-20', 'P014', 15, 450000.00, 14),
('2023-03-25', 'P015', 25, 375000.00, 15);

-- Insert para la tabla venta
INSERT INTO venta (fecha, terceroEmpleado_id, terceroCliente_id) VALUES 
('2023-01-10', 'T003', 'T001'),
('2023-01-15', 'T006', 'T004'),
('2023-01-20', 'T008', 'T007'),
('2023-01-25', 'T010', 'T008'),
('2023-01-30', 'T001', 'T010'),
('2023-02-05', 'T002', 'T011'),
('2023-02-10', 'T004', 'T013'),
('2023-02-15', 'T005', 'T014'),
('2023-02-20', 'T007', 'T015'),
('2023-02-25', 'T009', 'T002'),
('2023-03-05', 'T011', 'T005'),
('2023-03-10', 'T012', 'T006'),
('2023-03-15', 'T013', 'T009'),
('2023-03-20', 'T014', 'T003'),
('2023-03-25', 'T015', 'T012');

-- Insert para la tabla detalle_venta
INSERT INTO detalle_venta (factura_id, producto_id, cantidad, valor) VALUES 
(1, 'P001', 1, 3000000.00),
(2, 'P002', 2, 480000.00),
(3, 'P003', 3, 180000.00),
(4, 'P004', 5, 125000.00),
(5, 'P005', 1, 650000.00),
(6, 'P006', 2, 300000.00),
(7, 'P007', 10, 120000.00),
(8, 'P008', 1, 850000.00),
(9, 'P009', 1, 950000.00),
(10, 'P010', 2, 180000.00),
(11, 'P011', 3, 270000.00),
(12, 'P012', 1, 120000.00),
(13, 'P013', 2, 200000.00),
(14, 'P014', 3, 120000.00),
(15, 'P015', 5, 75000.00);

-- Insert para la tabla producto_proveedor
INSERT INTO producto_proveedor (tercero_id, producto_id) VALUES 
('T002', 'P001'),
('T002', 'P002'),
('T002', 'P003'),
('T005', 'P004'),
('T005', 'P005'),
('T007', 'P006'),
('T007', 'P007'),
('T009', 'P008'),
('T009', 'P009'),
('T012', 'P010'),
('T003', 'P011'),
('T006', 'P012'),
('T010', 'P013'),
('T013', 'P014'),
('T014', 'P015');