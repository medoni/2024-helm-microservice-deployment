// A reusable configuration service to fetch and manage dynamic configurations
export class ConfigService {
    constructor() {
        // @ts-ignore
        this.config = window.__env || {};
    }

    /**
     * Get a configuration value by key
     * @param {string} key - The key of the configuration value
     * @returns {any} - The configuration value
     */
    getConfig(key) {
      return this.config[key];
    }
}

export const configService = new ConfigService();
