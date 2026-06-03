export interface ApiError {
    type: string
    message: string
}

export async function apiPost<TRequest, TResponse>(endpoint: string, bodyData: TRequest): Promise<TResponse> {
    try {
        const response = await fetch(`${endpoint}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(bodyData),
            credentials: "include"
        })

        if (!response.ok) {
            let backendMessage = "Onbekende fout"
            try {
                const errorData = await response.json()
                backendMessage = errorData.message || errorData.error || JSON.stringify(errorData)
            } catch {
                backendMessage = response.statusText
            }

            throw {
                type: `Server Fout (${response.status})`,
                message: backendMessage
            } as ApiError
        }
        const data = await response.json() as TResponse
        return data
    } catch (error: any) {
        if (!error.type) {
            throw {
                type: "Netwerk Fout",
                message: "Kan geen verbinding maken met de server."
            } as ApiError
        }
        throw error
    }
}
export async function apiGet<TResponse>(endpoint: string): Promise<TResponse> {
    try {
        const response = await fetch(`${endpoint}`, {
            method: "GET",
            credentials: "include"
        })

        if (!response.ok) {
            let backendMessage = "Onbekende fout"
            try {
                const errorData = await response.json()
                backendMessage = errorData.message || errorData.error || JSON.stringify(errorData)
            } catch {
                backendMessage = response.statusText
            }

            throw {
                type: `Server Fout (${response.status})`,
                message: backendMessage
            } as ApiError
        }
        const data = await response.json() as TResponse
        return data
    } catch (error: any) {
        if (!error.type) {
            throw {
                type: "Netwerk Fout",
                message: "Kan geen verbinding maken met de server."
            } as ApiError
        }
        throw error
    }
}