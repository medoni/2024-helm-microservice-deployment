import playwright from 'eslint-plugin-playwright'

export default [
  {
    ...playwright.configs['flat/recommended'],
    files: ['**/*.js'],
    rules: {
      ...playwright.configs['flat/recommended'].rules,
      'playwright/no-focused-test': 'error',
      'playwright/no-skipped-test': 'warn',
      'playwright/valid-expect': 'error'
    },
  },
]
