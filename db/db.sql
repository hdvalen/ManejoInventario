CREATE DATABASE gestionInventario;
USE gestionInventario;

CREATE TABLE IF NOT EXISTS pais (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS region (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(50),
    pais_id INT,
    CONSTRAINT pais_region_FK FOREIGN KEY (pais_id) REFERENCES pais(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS ciudad (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(50),
    region_id INT,
    CONSTRAINT region_ciudad_FK FOREIGN KEY (region_id) REFERENCES region(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS empresa (
    id VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(50),
    ciudad_id INT,
    fecha_registro DATE,
    CONSTRAINT ciudad_empresa_FK FOREIGN KEY (ciudad_id) REFERENCES ciudad(id) ON DELETE CASCADE ON UPDATE CASCADE
); 

CREATE TABLE IF NOT EXISTS facturacion (
    id INT PRIMARY KEY AUTO_INCREMENT,
    fechaResolucion DATE,
    numInicio INT,
    numFinal INT,
    factura_actual INT
);

CREATE TABLE IF NOT EXISTS plan (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(30), 
    fecha_inicio DATE,
    fecha_fin DATE,
    descuento DECIMAL(10,2)
);

CREATE TABLE IF NOT EXISTS producto (
    id VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(50),
    stock INT,
    stockMin INT,
    stockMax INT,
    fecha_creacion DATE,
    fecha_actualizacion DATE,
    codigo_barra VARCHAR(50) UNIQUE
);

CREATE TABLE IF NOT EXISTS plan_producto (
    plan_id INT,
    producto_id VARCHAR(20),
    PRIMARY KEY (plan_id, producto_id),
    CONSTRAINT plan_id_FK FOREIGN KEY (plan_id) REFERENCES plan(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT producto_plan_FK FOREIGN KEY (producto_id) REFERENCES producto(id) ON DELETE CASCADE ON UPDATE CASCADE 
);

CREATE TABLE IF NOT EXISTS tipoMovCaja (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(50),
    tipoMovimiento ENUM('Salida', 'Entrada')
);

CREATE TABLE IF NOT EXISTS tipo_documento (
    id INT PRIMARY KEY AUTO_INCREMENT,
    descripcion VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS tipo_tercero (
    id INT PRIMARY KEY AUTO_INCREMENT,
    descripcion VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS tercero (
    id VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(50),
    apellidos VARCHAR(50),
    email VARCHAR(80) UNIQUE,
    tipo_documento_id INT,
    tipo_tercero_id INT,
    ciudad_id INT,
    CONSTRAINT tipo_documento_FK FOREIGN KEY (tipo_documento_id) REFERENCES tipo_documento(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT tipo_tercero_FK FOREIGN KEY (tipo_tercero_id) REFERENCES tipo_tercero(id) ON DELETE CASCADE ON UPDATE CASCADE, 
    CONSTRAINT ciudad_tercero_FK FOREIGN KEY (ciudad_id) REFERENCES ciudad(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS cliente (
    id INT PRIMARY KEY AUTO_INCREMENT,
    tercero_id VARCHAR(20),
    fecha_nacimiento DATE,
    fecha_compra DATE,
    CONSTRAINT tercero_cliente_FK FOREIGN KEY (tercero_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS proveedor (
    id INT PRIMARY KEY AUTO_INCREMENT,
    tercero_id VARCHAR(20),
    descuento DOUBLE,
    dia_pago INT,
    CONSTRAINT tercero_proveedor_FK FOREIGN KEY (tercero_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS eps (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS arl (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS tercero_telefono (
    id INT PRIMARY KEY AUTO_INCREMENT,
    numero VARCHAR(30),
    tercero_id VARCHAR(20),
    tipo_telefono ENUM('Fijo', 'Movil'),
    CONSTRAINT tercero_telefono_FK FOREIGN KEY (tercero_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS empleado (
    id INT PRIMARY KEY AUTO_INCREMENT,
    tercero_id VARCHAR(20),
    fecha_ingreso DATE,
    salario_base DOUBLE,
    eps_id INT,
    arl_id INT,
    CONSTRAINT tercero_empleado_FK FOREIGN KEY (tercero_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT empleado_eps_FK FOREIGN KEY (eps_id) REFERENCES eps(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT empleado_arl_FK FOREIGN KEY (arl_id) REFERENCES arl(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS movimientoCaja (
    id INT PRIMARY KEY AUTO_INCREMENT,
    fecha DATE,
    tipoMovimiento_id INT,
    valor DECIMAL(10,2),
    concepto TEXT,
    tercero_id VARCHAR(20),
    CONSTRAINT tipoMovimiento_id_FK FOREIGN KEY (tipoMovimiento_id) REFERENCES tipoMovCaja(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT tercero_movimiento_FK FOREIGN KEY (tercero_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS compra (
    id INT PRIMARY KEY AUTO_INCREMENT,
    terceroProveedor_id VARCHAR(20),
    fecha DATE,
    terceroEmpleado_id VARCHAR(20),
    DocCompra VARCHAR(30),
    CONSTRAINT proveedor_compra_FK FOREIGN KEY (terceroProveedor_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT empleado_compra_FK FOREIGN KEY (terceroEmpleado_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE
);


CREATE TABLE IF NOT EXISTS detalle_compra (
    id INT PRIMARY KEY AUTO_INCREMENT,
    fecha DATE,
    producto_id VARCHAR(20),
    cantidad INT,
    valor DECIMAL(10,2), 
    compra_id INT,
    CONSTRAINT producto_compra_FK FOREIGN KEY (producto_id) REFERENCES producto(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT compra_detalle_FK FOREIGN KEY (compra_id) REFERENCES compra(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS venta (
    factura_id INT PRIMARY KEY AUTO_INCREMENT,
    fecha DATE,
    terceroEmpleado_id VARCHAR(20),
    terceroCliente_id VARCHAR(20),
    CONSTRAINT empleado_venta_FK FOREIGN KEY (terceroEmpleado_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT cliente_venta_FK FOREIGN KEY (terceroCliente_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS detalle_venta (
    id INT PRIMARY KEY AUTO_INCREMENT,
    factura_id INT,
    producto_id VARCHAR(20),
    cantidad INT,
    valor DECIMAL(10,2),
    CONSTRAINT factura_venta_FK FOREIGN KEY (factura_id) REFERENCES venta(factura_id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT producto_venta_FK FOREIGN KEY (producto_id) REFERENCES producto(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS producto_proveedor (
    tercero_id VARCHAR(20),
    producto_id VARCHAR(20),
    PRIMARY KEY (tercero_id, producto_id),
    CONSTRAINT producto_proveedor_FK FOREIGN KEY (tercero_id) REFERENCES tercero(id) ON DELETE CASCADE ON UPDATE CASCADE, 
    CONSTRAINT proveedor_producto_FK FOREIGN KEY (producto_id) REFERENCES producto(id) ON DELETE CASCADE ON UPDATE CASCADE
);
