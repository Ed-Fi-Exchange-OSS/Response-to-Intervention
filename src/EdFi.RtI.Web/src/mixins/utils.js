export const utils = {
  methods: {

    /**
         * Given a number between 0.0 and 1.0, this method will simulate the probability
         * of returning true taking the input as a success percentage rate.
         *
         * Eg. The input 0.75 means this method will return true 75% of the times.
         *
         * @param { Number } percentage
         */
    chance (percentage) {
      const toInt = percentage * 100
      const random = this.randomInt(0, 100)
      return random > toInt
    },

    /**
         * Generates a random integer between the given range, inclusive.
         *
         * @param { Number } min
         * @param { Number } max
         * @returns { Number }
         */
    randomInt (min, max) {
      min = Math.ceil(min)
      max = Math.floor(max)
      return Math.floor(Math.random() * (max - min + 1)) + min
    }
  } // methods
}
