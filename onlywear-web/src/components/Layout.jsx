import { Link, Outlet } from 'react-router-dom'
import './Layout.css'

function Layout() {
  return (
    <div className="layout">
      <header className="header">
        <Link to="/" className="logo">
          OnlyWear
        </Link>
        <nav className="header-nav">
          <span className="header-link">Cuenta</span>
          <span className="header-link">Carrito</span>
        </nav>
      </header>
      <main className="main">
        <Outlet />
      </main>
    </div>
  )
}

export default Layout