export const forms = {
  data: () => ({
    rules: {
      assessments: {
        title: [
          (v) => !!v || ""
        ]

      } // assessment rules
    }
  })
}
