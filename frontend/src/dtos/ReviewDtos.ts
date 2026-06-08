export interface ItemTypeDto {
    value: number;
    name: string;
}

export interface ReviewListDto {
    reviews: ReviewDto[]
    totalCount: number
}

export interface ReviewDto {
    id: string
    title: string
    itemtype: string
    rating: number
    description: string
    groupIds: string[]
    name: string
}

