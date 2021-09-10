<template>
  <div>
    <!-- toolbar -->
    <v-app-bar>
      <v-toolbar-title>Reporting</v-toolbar-title>
    </v-app-bar><!-- toolbar -->

    <div class="small">
      <LineChart></LineChart>
    </div>

    <div class="mx-10 mb-5">
      <v-data-table
        :elevation="2"
        :headers="headers"
        :items="desserts"
        :items-per-page="5"
        class="elevation-1"></v-data-table>
    </div>
  </div>
</template>

<script>
import LineChart from "../components/charts/LineChart"

export default {
  components: {
    LineChart
  },
  data () {
    return {
      headers: [
        {
          text: "Baseline",
          align: "start",
          sortable: false,
          value: "baseline"
        },
        {
          text: "9/2",
          value: "val2",
          sortable: false
        },
        {
          text: "9/9",
          value: "val3",
          sortable: false
        },
        {
          text: "9/16",
          value: "val4",
          sortable: false
        },
        {
          text: "9/23",
          value: "val5",
          sortable: false
        },
        {
          text: "9/30",
          value: "val6",
          sortable: false
        },
        {
          text: "10/7",
          value: "val7",
          sortable: false
        },
        {
          text: "10/14",
          value: "val8",
          sortable: false
        },
        {
          text: "10/21",
          value: "val9",
          sortable: false
        },
        {
          text: "10/28",
          value: "val10",
          sortable: false
        },
        {
          text: "11/4",
          value: "val11",
          sortable: false
        },
        {
          text: "11/11",
          value: "val12",
          sortable: false
        },
        {
          text: "11/18",
          value: "val13",
          sortable: false
        },
        {
          text: "11/25",
          value: "val14",
          sortable: false
        },
        {
          text: "12/2",
          value: "val15",
          sortable: false
        }
      ],
      desserts: [
        {
          baseline: 10,
          val2: 10,
          val5: 11,
          val6: 10,
          val7: 12,
          val9: 8,
          val10: 15,
          val11: 12,
          val12: 11,
          val13: 11,
          val15: 17
        },
        {
          baseline: 7,
          val2: 7,
          val5: 4,
          val6: 3,
          val7: 5,
          val9: 5,
          val10: 2,
          val11: 4,
          val12: 4,
          val13: 6,
          val15: 2
        },
        {
          baseline: 0.56,
          val2: 0.56,
          val5: 0.56,
          val6: 0.56,
          val7: 0.56,
          val9: 0.56,
          val10: 0.56,
          val11: 0.56,
          val12: 0.56,
          val13: 0.56,
          val15: 0.56
        },
        {
          val5: 0.33,
          val6: 0.09,
          val7: 0.30,
          val9: 0.18,
          val10: 0.30,
          val11: 0.28,
          val12: 0.20,
          val13: 0.14,
          val15: 0.35
        },
        {
          val6: -1.17,
          val7: 0.58,
          val9: -0.68,
          val10: 0.40,
          val11: 0.33,
          val12: 0.19,
          val13: 0.11,
          val15: 0.41
        }
      ]
    }
  }

}
</script>
<style>
  .small {
    max-width: 800px;
    margin: 50px auto;
  }
</style>
