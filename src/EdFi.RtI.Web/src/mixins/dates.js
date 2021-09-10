import moment from "moment"

export const dates = {
  methods: {

    /**
         * Extracts the current time from the now Date and embeds it into the provided Date string
         * @param { String } dateStr
         */
    embedNowTime (dateStr) {
      const now = new Date()
      const dateWithTime = new Date(dateStr)

      dateWithTime.setUTCHours(now.getUTCHours())
      dateWithTime.setUTCMinutes(now.getUTCMinutes())
      dateWithTime.setUTCSeconds(now.getUTCSeconds())
      dateWithTime.setUTCMilliseconds(now.getUTCMilliseconds())

      return dateWithTime
    },

    /**
         * Gives a "MM/DD/YYYY" formatting to a complete local date string
         *
         * @param { String } dateStr
         * @returns { String }
         */
    toLocalShort (dateStr) {
      //return moment(dateStr).format('MM/DD/YYYY');
      const dateFormatted = this.formatDate(new Date(dateStr).toISOString().substr(0, 10))
      return dateFormatted
    },

    /**
         * Gives a "MM/DD/YYYY hh:mm:ss" formatting to a complete local date string
         *
         * @param { String } dateStr
         * @returns { String }
         */
    toLocalShortWithTime (dateStr) {
      const format = "YYYY-MM-DD HH:mm:ss"
      return moment(dateStr).format(format)
    },

    formatDate (date) {
      if (!date) {return null}

      const [
        year,
        month,
        day
      ] = date.split("-")
      return `${month}/${day}/${year}`
    },

    /**
         * Gives a complete local date string formatting to a "MM/DD/YYYY" date string
         * @param { String } dateStr
         * @returns { String }
         */
    fromLocalShort (dateStr) {
      return moment(dateStr).toString()
    }
  }
}
