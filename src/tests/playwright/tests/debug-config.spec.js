import { test, expect } from '@playwright/test';

test('Debug: Check frontend config', async ({ page }) => {
  // Navigate to the app first
  await page.goto('/');

  // Fetch the config.js file content via request (not navigation)
  const configResponse = await page.request.get('/config.js');
  const configContent = await configResponse.text();

  console.log('=== config.js content ===');
  console.log(configContent);

  // Now check window.__env in the browser context (on the main page)
  const envConfig = await page.evaluate(() => {
    return window.__env;
  });

  console.log('=== window.__env ===');
  console.log(JSON.stringify(envConfig, null, 2));

  // Show what we got
  if (!envConfig) {
    console.error('ERROR: window.__env is undefined! config.js was not loaded or did not execute properly.');
    console.log('This means the config.js file either:');
    console.log('1. Does not exist at /config.js');
    console.log('2. Has a syntax error');
    console.log('3. The envsubst substitution failed');
    throw new Error('window.__env is undefined - config.js failed to load');
  }

  // Verify that pizzaApiUrl is set
  expect(envConfig).toHaveProperty('pizzaApiUrl');
  console.log(`API URL: ${envConfig.pizzaApiUrl}`);

  // Try to fetch from the API URL
  console.log(`=== Testing API connectivity ===`);
  try {
    const apiResponse = await page.request.get(`${envConfig.pizzaApiUrl}/v1/Menu/active`);
    console.log(`API Status: ${apiResponse.status()}`);
    console.log(`API OK: ${apiResponse.ok()}`);

    if (apiResponse.ok()) {
      const data = await apiResponse.json();
      console.log(`API Response: ${JSON.stringify(data).substring(0, 200)}...`);
    } else {
      console.log(`API Error: ${await apiResponse.text()}`);
    }
  } catch (error) {
    console.error(`API Fetch Error: ${error.message}`);
  }
});
