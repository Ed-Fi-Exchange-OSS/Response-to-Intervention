const express = require("express")

const history = require("connect-history-api-fallback")
const app = express()
const defaultConfig = require("./wwwroot/app.config.json")
require("dotenv").config()

const mergeEnvVars = require("./merge-env-vars")
const config = mergeEnvVars(defaultConfig)

const staticFileMiddleware = express.static("wwwroot")

app.use(`${config.app.basePath}/config.json`, (_, res) => {
  res.json(config)
})

app.use(history({
  index: `${config.app.basePath}/index.html`,
  verbose: true
}))

app.use(config.app.basePath, staticFileMiddleware)

app.listen(process.env.PORT || 8590, () => {
  console.log(`Server running at http://localhost:${process.env.PORT || 8590}${config.app.basePath}`)
})
