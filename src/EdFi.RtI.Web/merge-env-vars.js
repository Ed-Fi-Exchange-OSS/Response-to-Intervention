const { mapKeys, set, defaultsDeep } = require("lodash")

function mergeEnv (defaultConf) {
  const toUseConf = {
  }
  mapKeys(process.env, (v, k) => {
    if (k.includes("EG_")) {
      if (k.includes("__")) {
        const [
          sp1,
          ...sp2
        ] = k.substring(3).split("__")
        set(toUseConf, `${sp1}.${sp2.join("__")}`, v)
      }
      else {
        set(toUseConf, k.substring(3), v)
      }
    }
  })
  return defaultsDeep(toUseConf, defaultConf)
}

module.exports = mergeEnv
