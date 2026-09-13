import axios from 'axios'

const client = axios.create({
  baseURL: 'http://localhost:5197/api',
})

export default client