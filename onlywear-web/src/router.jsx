import { createBrowserRouter } from 'react-router-dom'
import Layout from './components/Layout.jsx'
import Catalogo from './pages/Catalogo.jsx'
import ProductoDetalle from './pages/ProductoDetalle.jsx'

const router = createBrowserRouter([
  {
    element: <Layout />,
    children: [
      { path: '/', element: <Catalogo /> },
      { path: '/producto/:id', element: <ProductoDetalle /> },
    ],
  },
])

export default router