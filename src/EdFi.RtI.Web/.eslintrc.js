module.exports = {
  root: true,
  env: {
    node: true
  },
  extends: [
    "plugin:vue/recommended"
  ],
  parserOptions: {
    parser: "babel-eslint"
  },
  rules: {
    indent: [
      "error",
      2
    ],
    "comma-dangle": "error",
    "computed-property-spacing": "error",
    "arrow-spacing": "error",
    "key-spacing": "error",
    "keyword-spacing": "error",
    "no-multiple-empty-lines": [
      "error",
      {
        max: 1,
        maxEOF: 1
      }
    ],
    "switch-colon-spacing": "error",
    "no-whitespace-before-property": "error",
    "space-infix-ops": "error",
    "no-trailing-spaces": "error",
    "space-before-function-paren": "error",
    "space-in-parens": "error",
    "array-bracket-newline": [
      "error",
      {
        multiline: true,
        minItems: 1
      }
    ],
    camelcase: "warn",
    "array-bracket-spacing": "error",
    "arrow-parens": "error",
    "function-paren-newline": [
      "error",
      "multiline"
    ],
    "array-element-newline": [
      "error",
      {
        minItems: 1,
        multiline: true
      }
    ],
    "vars-on-top": "error",
    "prefer-const": "error",
    "prefer-rest-params": "error",
    "prefer-spread": "error",
    "no-lonely-if": "error",
    "no-useless-rename": "error",
    "no-useless-concat": "error",
    "no-useless-return": "error",
    "no-var": "error",
    semi: [
      "error",
      "never"
    ],
    "one-var": [
      "error",
      "never"
    ],
    "block-scoped-var": "error",
    "object-curly-spacing": [
      "error",
      "always"
    ],
    "object-property-newline": [
      "error",
      {
        allowAllPropertiesOnSameLine: false
      }
    ],
    "object-curly-newline": [
      "error",
      {
        ObjectExpression: "always",
        ImportDeclaration: "never"
      }
    ],
    "no-bitwise": "error",
    "require-await": "error",
    "quote-props": [
      "error",
      "as-needed"
    ],
    "sort-vars": "error",
    "wrap-iife": "error",
    "func-call-spacing": [
      "error",
      "never"
    ],
    "eol-last": "error",
    "dot-notation": "error",
    "dot-location": [
      "error",
      "property"
    ],
    curly: [
      "error",
      "all"
    ],
    "lines-between-class-members": "error",
    "comma-spacing": "error",
    quotes: [
      "error",
      "double",
      "avoid-escape"
    ],
    "vue/component-name-in-template-casing": [
      "error",
      "PascalCase",
      {
        registeredComponentsOnly: true
      }
    ],
    "vue/max-attributes-per-line": [
      "error",
      {
        singleline: 1,
        multiline: {
          max: 1,
          allowFirstLine: false
        }
      }
    ],

    "vue/html-closing-bracket-spacing": "warn",
    "vue/html-closing-bracket-newline": [
      "error",
      {
        singleline: "never",
        multiline: "never"
      }
    ],
    "vue/prop-name-casing": "error",
    "vue/html-self-closing": "off",
    "vue/no-v-html": "off",
    "vue/valid-v-slot": "off",
    "vue/valid-v-bind-sync": "off"
  },
  overrides: [
    {
      files: [
        "**/__tests__/*.{j,t}s?(x)",
        "**/tests/unit/**/*.spec.{j,t}s?(x)"
      ],
      env: {
        jest: true
      }
    }
  ]
}
