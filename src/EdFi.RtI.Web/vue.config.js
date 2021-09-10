const { DefinePlugin } = require("webpack")
const path = require("path")
const VersionFile = require("webpack-version-file-plugin")
const { version } = require("./package.json")

const defaultConfig = require("./public/app.config.json")
const mergeEnvVars = require("./merge-env-vars")
const mev = mergeEnvVars(defaultConfig)

module.exports = {
  productionSourceMap: false,
  publicPath: mev.app.basePath == "" ? "/" : mev.app.basePath,
  outputDir: "wwwroot/",
  assetsDir: "src/",
  devServer: {
    port: 8590
  },
  configureWebpack: {
    resolve: {
      alias: {
        Api: path.resolve(__dirname, "src/api/"),
        Components: path.resolve(__dirname, "src/components/"),
        Constants: path.resolve(__dirname, "src/constants"),
        Assets: path.resolve(__dirname, "src/assets"),
        Container: path.resolve(__dirname, "src/container"),
        Views: path.resolve(__dirname, "src/views")
      },
      extensions: [
        "*",
        ".js",
        ".vue",
        ".json"
      ]
    },
    plugins: [
      new DefinePlugin({
        VERSION: JSON.stringify(version)
      }),
      new VersionFile({
        packageFile: path.join(__dirname, "package.json"),
        outputFile: path.join("wwwroot", "version.json")
      })
    ]
  },
  transpileDependencies: [
    "vuetify"
  ]

}
