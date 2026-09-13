import client from './client'

export async function obtenerProductos() {
  const { data } = await client.get('/productos')
  return data
}

export async function obtenerProductoPorId(id) {
  const { data } = await client.get(`/productos/${id}`)
  return data
}