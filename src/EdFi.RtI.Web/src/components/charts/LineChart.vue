<script>
import { Line } from "vue-chartjs"

export default {
  extends: Line,
  data () {
    return {
      chartData: {
        labels: [
          "Sep",
          "Oct",
          "Nov",
          "Dec",
          "Jan",
          "Feb",
          "Mar",
          "Apr",
          "May",
          "Jun",
          "Jul"
        ],
        datasets: [
          {
            label: "Score",
            data: [
              16,
              25,
              7,
              14,
              41,
              37,
              3,
              27,
              32,
              41,
              34
            ],
            fill: false,
            borderColor: "black",
            backgroundColor: "black",
            borderWidth: 1
          },
          {
            label: "Projection",
            data: [
              10,
              11,
              12,
              13,
              14,
              15,
              16,
              17,
              18,
              19,
              20
            ],
            borderColor: "red",
            backgroundColor: "red",
            fill: false
          },
          {
            label: "Aimline",
            data: [
              10,
              12,
              14,
              16,
              18,
              20,
              22,
              24,
              26,
              28,
              30
            ],
            borderColor: "gray",
            backgroundColor: "gray",
            fill: false
          }
        ]
      },
      options: {
        scales: {
          yAxes: [
            {
              ticks: {
                beginAtZero: true
              },
              gridLines: {
                display: true
              }
            }
          ],
          xAxes: [
            {
              gridLines: {
                display: false
              }
            }
          ]
        },
        legend: {
          display: true
        },
        responsive: true,
        maintainAspectRatio: false
      }
    }
  },
  mounted () {
    this.renderChart(this.chartData, this.options)
  }
}
</script>

<style>
</style>
