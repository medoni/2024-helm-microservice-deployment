import { test, expect } from '@playwright/test';

test('Debug: Check frontend config', async ({ page }) => {
  // Navigate to the app
  await page.goto('/');

  // Fetch the config.js file
  const configResponse = await page.goto('/config.js');
  const configContent = await configResponse.text();

  console.log('=== config.js content ===');
  console.log(configContent);

  // Check window.__env in the browser context
  const envConfig = await page.evaluate(() => {
    return window.__env;
  });

  console.log('=== window.__env ===');
  console.log(JSON.stringify(envConfig, null, 2));

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
