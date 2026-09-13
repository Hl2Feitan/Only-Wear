import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { obtenerProductos } from '../api/productos'
import './Catalogo.css'

function Catalogo() {
  const [productos, setProductos] = useState([])
  const [cargando, setCargando] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    obtenerProductos()
      .then(setProductos)
      .catch(() => setError('No se pudieron cargar los productos.'))
      .finally(() => setCargando(false))
  }, [])

  if (cargando) {
    return <p className="estado">Cargando...</p>
  }

  if (error) {
    return <p className="estado">{error}</p>
  }

  if (productos.length === 0) {
    return <p className="estado">No hay productos disponibles por ahora.</p>
  }

  return (
    <section className="catalogo">
      <h1 className="catalogo-titulo">Catálogo</h1>
      <div className="productos-grid">
        {productos.map((producto) => (
          <Link key={producto.id} to={`/producto/${producto.id}`} className="producto-card">
            {producto.imagenUrl ? (
              <img className="producto-imagen" src={producto.imagenUrl} alt={producto.nombre} />
            ) : (
              <div className="producto-placeholder" aria-hidden="true" />
            )}
            <div className="producto-info">
              <h2 className="producto-nombre">{producto.nombre}</h2>
            </div>
          </Link>
        ))}
      </div>
    </section>
  )
}

export default Catalogo