class TagService {

    static async getAOData() {
        const response = await fetch('http://localhost:5083/api/tag/ao');
        const data = await response.json();
        return data;
    }

    static async getDOData() {
        const response = await fetch('http://localhost:5083/api/tag/do');
        const data = await response.json();
        return data;
    }

    static async getAIData() {
        const response = await fetch('http://localhost:5083/api/tag/ai');
        const data = await response.json();
        return data;
    }

    static async getDIData() {
        const response = await fetch('http://localhost:5083/api/tag/di');
        const data = await response.json();
        return data;
    }
}

export default TagService;

