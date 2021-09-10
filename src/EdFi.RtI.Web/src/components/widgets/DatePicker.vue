<template>
  <v-menu
    ref="menu"
    v-model="isDatePickerOpened"
    :close-on-content-click="false"
    :return-value.sync="date"
    transition="scale-transition"
    offset-y
    min-width="290px">
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        v-model="dateFormatted"
        :label="labelValue"
        persistent-hint
        prepend-icon="mdi-calendar"
        readonly
        :disabled="isDisabled"
        v-bind="attrs"
        @blur="date = parseDate(dateFormatted)"
        v-on="on"></v-text-field>
    </template>
    <v-date-picker
      v-model="date"
      no-title
      scrollable>
      <v-spacer></v-spacer>
      <v-btn
        text
        color="primary"
        @click="isDatePickerOpened = false">
        Cancel
      </v-btn>
      <v-btn
        text
        color="primary"
        @click="$refs.menu.save(date); sendDate();">
        OK
      </v-btn>
    </v-date-picker>
  </v-menu>
</template>

<script>
/* eslint-disable */
import { dates } from '../../mixins/mixins';

export default {

    props: {
        value: String,
        labelText: String,
        isDisabled: Boolean,
    }, // props

    data: vm => ({
        isDatePickerOpened: false,
        dateModel: new Date(),
        labelValue: "",

        date: new Date().toISOString().substr(0, 10),
        dateFormatted: vm.formatDate(new Date().toISOString().substr(0, 10)),
        menu: false,
        modal: false,
        menu2: false,
    }), // data

    computed: {
        computedDateFormatted () {
            return this.formatDate(this.date)
        },
    }, // computed

    watch: {
        date () {
            this.dateFormatted = this.formatDate(this.date)
        },
    }, //watch

    mounted() {
        this.date= new Date(this.value).toISOString().substr(0, 10);
        this.dateFormatted= this.formatDate(new Date(this.value).toISOString().substr(0, 10));
        this.labelValue = this.labelText;
        this.sendDate();
    }, // mounted

    methods: {
        formatDate (date) {
            if (!date) return null

            const [year, month, day] = date.split('-')
            return `${month}/${day}/${year}`
        },

        parseDate (date) {
            if (!date) return null

            const [month, day, year] = date.split('/')
            return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
        },

        sendDate(event) {
            //let date = new Date(this.date).toISOString(); // TODO Uncomment to use previous solution
            //this.$emit('input', date); // TODO Uncomment to use previous solution
            let dateWithTime = dates.methods.embedNowTime(this.date);
            this.$emit('input', dateWithTime.toISOString());
        },
    }, // methods

    mixins: [ dates ],
}
</script>
