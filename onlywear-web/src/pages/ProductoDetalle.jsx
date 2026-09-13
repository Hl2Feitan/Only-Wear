import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { obtenerProductoPorId } from '../api/productos'
import './ProductoDetalle.css'

function formatearPrecio(precio) {
  return `$${Number(precio).toFixed(2)}`
}

function ProductoDetalle() {
  const { id } = useParams()
  const [producto, setProducto] = useState(null)
  const [error, setError] = useState(null)
  const [talleSeleccionado, setTalleSeleccionado] = useState('')

  useEffect(() => {
    let activo = true

    obtenerProductoPorId(id)
      .then((data) => {
        if (!activo) return
        setProducto(data)
        setError(null)
        if (data.precios && data.precios.length > 0) {
          setTalleSeleccionado(data.precios[0].talle)
        }
      })
      .catch(() => {
        if (!activo) return
        setProducto(null)
        setError('No se pudo cargar el producto.')
      })

    return () => {
      activo = false
    }
  }, [id])

  const cargando = producto === null && error === null

  if (cargando) {
    return <p className="estado">Cargando...</p>
  }

  if (error) {
    return <p className="estado">{error}</p>
  }

  if (!producto) {
    return <p className="estado">Producto no encontrado.</p>
  }

  const precioSeleccionado = producto.precios?.find(
    (precio) => precio.talle === talleSeleccionado,
  )

  return (
    <section className="detalle">
      <div className="detalle-imagen">
        {producto.imagenUrl ? (
          <img className="detalle-img" src={producto.imagenUrl} alt={producto.nombre} />
        ) : (
          <div className="detalle-placeholder" aria-hidden="true" />
        )}
      </div>

      <div className="detalle-info">
        <h1 className="detalle-nombre">{producto.nombre}</h1>
        <p className="detalle-descripcion">{producto.descripcion}</p>

        {producto.precios && producto.precios.length > 0 && (
          <div className="detalle-talles">
            <label className="detalle-label" htmlFor="talle">
              Talle
            </label>
            <select
              id="talle"
              className="detalle-select"
              value={talleSeleccionado}
              onChange={(event) => setTalleSeleccionado(event.target.value)}
            >
              {producto.precios.map((precio) => (
                <option key={precio.id} value={precio.talle}>
                  {precio.talle}
                </option>
              ))}
            </select>
            {precioSeleccionado && (
              <p className="detalle-precio">{formatearPrecio(precioSeleccionado.precio)}</p>
            )}
          </div>
        )}

        <button className="detalle-boton" type="button" disabled>
          Agregar al carrito
        </button>
      </div>
    </section>
  )
}

export default ProductoDetalle